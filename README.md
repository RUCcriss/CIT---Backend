# Github til cit backend

## Dotnet

We use dotnet from the CLI for various things. This is a collection of how we did some of the things so we can remember later.

### Create new projects (components)

When creating the server, client and util components, we used dotnet to create each of the projects. With server as example, we did the following from the repos root:

```console
dotnet new console Server
```

### Running server and client

Both are run from the CLI via the following (server as example):

```console
dotnet run --project Server
```
