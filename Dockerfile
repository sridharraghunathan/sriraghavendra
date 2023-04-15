ARG PWD_DIR=/usr/src/app

#Building the .NETCORE API we are pulling the sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS backend-build
ARG PWD_DIR
WORKDIR ${PWD_DIR}
#since we need restore the package multiple times we are doing before.
COPY ./API/*.csproj ./
RUN dotnet restore
#copy the actual code.
COPY ./API/* ./
#Copying solution file gitignore .dockerignore and pipeline file
COPY . .
#remove the angular app folder
RUN rm -Rf ${PWD_DIR}/raghavendra-app
#publish the app to out folder 
RUN dotnet publish SriRaghavendraApp.sln -c Release -o out

#we can use the node better image if required.
FROM node:16 AS ui-build
ARG PWD_DIR
ENV PATH ${PWD_DIR}/node_modules/.bin:$PATH
WORKDIR ${PWD_DIR}
#copy only package file to avoid the npm rebuild when we run again.
COPY ./raghavendra-app/package.json ./angularapp/
RUN cd angularapp/ && npm config set legacy-peer-deps true && npm install @angular/cli@11 && npm install --save --legacy-peer-deps && npm run build:prod
#copy the build file in the dist folder
COPY ./raghavendra-app/dist/raghavendra-app ./angularapp/
#aspnet image for running the .NET APPLICATION AND WWWROOT has angular build code

FROM mcr.microsoft.com/dotnet/aspnet:5.0 
ARG PWD_DIR
WORKDIR /final-code
# moving the .net core build code to root directory
COPY --from=backend-build ${PWD_DIR}/out/* ./
#copying the angular app to wwwroot folder 
COPY --from=ui-build ${PWD_DIR}/angularapp/dist/raghavendra-app ./wwwroot/
EXPOSE 80
ENTRYPOINT ["dotnet","API.dll"]