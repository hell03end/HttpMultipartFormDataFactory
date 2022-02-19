# HttpMultipartFormDataFactory

A small library for creating multipart/form-data content from flat stucture objects.

This project is licensed under the [MIT license](LICENSE).

## Usage

Create an instance of the HttpMultipartFormDataFactory class, then pass it a request object:

```cs
var multipartFormDataFactory = new HttpMultipartFormDataFactory();
var format = await multipartFormDataFactory.Create(factory, token);
```

This will return a System.Net.Http.MultipartFormDataContent instance.
