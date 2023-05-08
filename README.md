# FairWear Backend


## Databases

### Brand and Product Database

#### Build image

To build the image for the brand and product database run the following command at the root of the project :

```shell
docker build --no-cache -t brands_and_products_db -f ./Databases/BrandsAndProductsDatabase/Dockerfile . 
```

#### Run container

You'll need to set the following environment variables :

```shell
export POSTGRES_USER=""
export POSTGRES_PASSWORD=""
export POSTGRES_BRAND_DATABASE=""
```

Then to run the container run the following command :

```shell
docker run -p 5432:5432 -t --rm --name brand_database -e POSTGRES_USER=$POSTGRES_USER -e POSTGRES_PASSWORD=$POSTGRES_PASSWORD -e POSTGRES_DB=$POSTGRES_DB  brands_and_products_db
```

## Code Coverage Script

This script is designed to run unit tests with code coverage and generate a code coverage report using ReportGenerator. The script is written in Bash and requires the .NET Core runtime to be installed on the system. The script takes two arguments:

`-p|--project`: The path to the project's test directory  
`--clean`: Optional flag to delete generated files

### Usage

To use the script, navigate to the directory containing the script and run the following command:

```shell
./code_coverage.sh -p|--project <project_test_path> [--clean]
```
Replace `<project_test_path>` with the path to the project's test directory. The `--clean` flag is optional and can be used to delete any previously generated files.

### Steps Performed

The script performs the following steps:

1) Parses the command-line arguments and checks if the required `-p|--project` argument is provided.
2) Runs the unit tests with code coverage using the `dotnet test` command.
3) Generates a code coverage report using ReportGenerator and saves it to a directory called `coveragereport`.
4) Opens the generated code coverage report in the default web browser.
5) If the `--clean` flag is used, the script will delete the `coveragereport` directory, the `coverage.cobertura.xml` file, and the `TestResults` directory.