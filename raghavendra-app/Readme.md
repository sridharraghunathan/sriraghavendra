1. Build the code which copy the code to the API folder of .NET CORE need to be run in angular folder
   ng build --prod
   A) Local testing dotnet run 
   B) Remove the out folder before publishing
2. Publish the code to out directory run in root folder.
   dotnet publish SriRaghavendraApp.sln -c Release -o out
3. we can run below git commands
   git add .
   git commit -m "commit messages"
   git push
   password get from the azure repo by selecting the clone
4. release Pipeline select only the out folder which has all the published code
