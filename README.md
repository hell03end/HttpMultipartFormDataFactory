# HttpMultipartFormDataFactory

[![build](https://github.com/hell03end/HttpMultipartFormDataFactory/actions/workflows/build.yml/badge.svg)](https://github.com/hell03end/HttpMultipartFormDataFactory/actions/workflows/build.yml)
[![NuGet version](https://badge.fury.io/nu/HttpMultipartFormDataFactory.svg)](https://badge.fury.io/nu/HttpMultipartFormDataFactory)

A small library for creating multipart/form-data content from flat structure objects.

This project is licensed under the [MIT license](LICENSE).

## Usage

Create an instance of the HttpMultipartFormDataFactory class, then pass it a request object:

```cs
var multipartFormDataFactory = new HttpMultipartFormDataFactory();
var format = await multipartFormDataFactory.Create(factory, token);
```

This will return a System.Net.Http.MultipartFormDataContent instance.
