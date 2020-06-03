# Web

## Debugging the project
All the settings should be good to debug with Visual Studio. On debug mode,
dotnet should use the appsettings.Development.json with the right configuration.

## Building the Docker image
To build the docker image, run on this folder:
`docker build -t -web:0.0.1 .` replacing `0.0.1` your version.

## Running the Image
With the image built, run it with:
`docker run --name w1 -p 21400:80 -web:0.0.1` replacing `0.0.1` with
your version.


## Other useful commands
Other useful commands are:
```sh
docker image rm -web:0.0.1 -f				# removes the image
docker container rm -f w1					# removes the container
```

