# management-system-api

## Management.System.Api e Docker

Primeiraimente, execute o comando "docker-compose up -d" no terminal dentro da aplicação Management.System.Api para subir o Docker com o .NET e o PostgreSQL.

Execute o comando "docker-compose down" caso queira destruir o container.

### Subindo o banco de dados

Execute o comando "dotnet ef database update" na CLI ou "Update-Database" no Package Manager Console para atualizar as tabelas do banco. 

Os scripts de SELECT para visualizar as tabelas está localizado na pasta src/Api/Scripts.

### Acompanhando o banco de dados usando O Dbeaver ou algum outro aplicativo de banco de dados

Instale o aplicativo e configure o banco de dados inserindo as informações de variáveis de ambiente no arquivo appsettings.json.

### Iniciando a aplicação

Abra o navegador e cole esse link: https://localhost:5001/swagger/index.html. Essa URL irá abrir a interface do Swagger para testar a API.

### Acompanhando os Logs

Acompanhe os arquivos de logs na pasta src/Api/Logs.

### Cobertura de Testes

Execute o comando "dotnet test --collect: "XPlat Code Coverage"", depois "reportgenerator "-reports:.\tests\UnitTests\Management.System.UnitTests\TestResults\nome-do-arquivo-gerado\coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html", substituindo "nome-do-arquivo-gerado" pelo arquivo gerado na pasta TestResults.

Em seguida, navegue até a pasta coveragereport/ e, por fim, execute "start ./index.htm" para abrir o relatório no navegador.