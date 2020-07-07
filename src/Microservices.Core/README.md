# Microservices.Core
Shared packages between AspNetMicroservices.

Currently published on GitHub at:
[github.com/hd9/aspnet-microservices](https://github.com/hd9/aspnet-microservices).

## Building your packages
To build your package, make sure you add a new [Personal Access Token](https://github.com/settings/tokens)
and set it as an environment variable as: `GITHUB_API_KEY=<your-token>`.
Both `docker build` and `docker-compose build` rely on that environment
variable to be present to build the images.

Also, don't forget to add your username on all `nuget.config` files.

## Pushing our packages
To push this package to your repo do:
1. Adjust your push settings on GitHub at
   [github.com/settings/tokens](https://github.com/settings/tokens)
   and check `write:packages`
2. build your package with `dotnet pack`
3. push to your GitHub pkg repository.


For more information, check
[Configuring dotnet CLI for use with GitHub Packages](https://help.github.com/en/packages/using-github-packages-with-your-projects-ecosystem/configuring-dotnet-cli-for-use-with-github-packages).
