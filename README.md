# CoffeeAPIMinimal

This is a project where a minimal .Net 6 Core API has been built to act as an imaginary coffee machine. It uses Redis to use a cache based counter to track the number of times the API has been called. 

## Installation

Clone the project and install all the dependencies on VS 2022. You will need to use a Redis docker instance to use Redis, the command for which has been given below.

```d
docker run --name redis -p 6379:6379 -d redis:latest
```
The command will install a docker Redis instance with 6379 as the port number and "Redis" as the instance name.
## Usage

```powershell
dotnet run 
```
