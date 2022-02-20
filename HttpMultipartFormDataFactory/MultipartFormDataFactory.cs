using System.Collections.Concurrent;
using System.Net.Http.Headers;
using HttpMultipartFormDataFactory.Models;
using Microsoft.AspNetCore.Http;

namespace HttpMultipartFormDataFactory;

public class MultipartFormDataFactory
{
    private ConcurrentDictionary<Type, PropertyData[]>? PropertiesCache { get; }

    public MultipartFormDataFactory(bool useCache = true)
    {
        PropertiesCache = useCache
            ? new ConcurrentDictionary<Type, PropertyData[]>()
            : null;
    }

    public static MultipartFormDataFactory Default => new();

    /// <summary>
    /// Creates MultipartFormDataContent
    /// </summary>
    /// <param name="request">Request DTO</param>
    /// <param name="token">Cancellation token</param>
    /// <typeparam name="T">Type of request DTO</typeparam>
    /// <returns>Filled MultipartFormDataContent</returns>
    public async Task<MultipartFormDataContent> Create<T>(T request, CancellationToken token)
    {
        var properties = GetPropertiesData<T>();

        var requestData = new List<FieldData>();
        var propsMap = properties.ToDictionary(x => x, x => x.PropertyInfo.GetValue(request));

        foreach (var item in propsMap.Where(item => item.Value is not null))
        {
            if (item.Key.IsFile)
            {
                if (item.Value is not IFormFile val)
                {
                    continue;
                }

                var data = await GetStreamContentField(val, item.Key.Name, token);

                requestData.Add(data);
            }
            else if (item.Key.IsCollection)
            {
                switch (item.Value)
                {
                    case IFormFile[] val:
                        await AddFormFilesData(val, item.Key.Name, requestData, token);
                        break;
                    case int[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case long[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case double[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case float[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case bool[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case decimal[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case byte[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case char[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case short[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case Guid[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case DateTimeOffset[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    case object[] val:
                        AddMultipleStringData(val, item.Key.Name, requestData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(item.Value), item.Value, "Value is not supported");
                }
            }
            else
            {
                requestData.Add(GetStringContentField(item.Value, item.Key.Name));
            }
        }

        var content = new MultipartFormDataContent();
        foreach (var data in requestData)
        {
            if (data.FileName is null)
            {
                content.Add(data.Value, data.ParamName);
            }
            else
            {
                content.Add(data.Value, data.ParamName, data.FileName);
            }
        }

        return content;
    }

    #region Private

    private PropertyData[] GetPropertiesData<T>()
    {
        var requestType = typeof(T);

        if (PropertiesCache?.TryGetValue(requestType, out var properties) ?? false)
        {
            return properties ?? Array.Empty<PropertyData>();
        }

        properties = requestType.GetProperties()
            .Select(x => new PropertyData
            {
                PropertyInfo = x,
                IsFile = typeof(IFormFile).IsAssignableFrom(x.PropertyType),
                IsCollection = x.PropertyType.GetInterfaces()
                                   .Any(y => y.IsGenericType &&
                                             y.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                               && !typeof(string).IsAssignableFrom(x.PropertyType),
                Name = x.Name,
            })
            .ToArray();

        PropertiesCache?.TryAdd(requestType, properties);

        return properties;
    }

    private static async Task AddFormFilesData<T>(
        IEnumerable<T> value,
        string paramName,
        List<FieldData> requestData,
        CancellationToken token)
    {
        var values = value as IFormFile[] ?? Array.Empty<IFormFile>();
        if (!values.Any())
        {
            return;
        }

        var data = await Task.WhenAll(values.Select(async x => await GetStreamContentField(x, paramName, token)));

        if (data.Any())
        {
            requestData.AddRange(data);
        }
    }

    private static async Task<FieldData> GetStreamContentField(
        IFormFile file,
        string paramName,
        CancellationToken token)
    {
        var stream = new MemoryStream();

        await file.CopyToAsync(stream, token);

        stream.Position = 0;

        var streamContent = new StreamContent(stream);

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        return new FieldData
        {
            Value = streamContent,
            ParamName = paramName,
            FileName = file.FileName,
        };
    }

    private static FieldData GetStringContentField<T>(T value, string paramName)
    {
        var data = value?.ToString() ?? string.Empty;

        var content = new StringContent(data);

        content.Headers.ContentLength = data.Length;

        return new FieldData
        {
            Value = content,
            ParamName = paramName,
        };
    }

    private static void AddMultipleStringData<T>(IEnumerable<T>? value, string paramName, List<FieldData> requestData)
    {
        var values = value as T[] ?? value?.ToArray() ?? Array.Empty<T>();
        if (!values.Any())
        {
            return;
        }

        var data = values.Select(x => GetStringContentField(x, paramName)).ToArray();

        if (data.Any())
        {
            requestData.AddRange(data);
        }
    }

    #endregion
}
