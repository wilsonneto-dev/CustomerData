Vídeo mostrando rodando:
https://youtu.be/R8199Ub4ou0

## Desafio

* OK - Criar um site com 4 páginas estáticas simples
  - Home - ok
  - Landing - ok
  - Checkout do Pedido - ok
  - Confirmação do Pedido - ok

* OK - Criar um JavaScript que efetue a coleta e envie os Dados para a API
  - IP, Nome, Browser, Parametros de Página - ok
  - Deve ser um simples JavaScript que coleta os dados do Browser e envie para API de Comportamento - ok
  - Deve ser entregue em todas as Páginas estáticas - ok

* OK - Criar uma Web API para coletar comportamento do cliente - ok
  - Comportamento deve ter os seguintes dados - ok
    - IP
    - Nome da Página
    - Browser
    - Parâmetros da Página
  - Deve ser Armazenado em uma Fila do RabbitMQ - OK
  
- Criar um Robô que consuma a fila no RabbitMQ - OK
  - Deve ler os Eventos na Fila do RabbitMQ - OK
  - Salvar os Dados em duas Infras diferentes - OK
    - Couchbase - OK
    - SQL Server - Entity - OK

- Criar uma Web API para ler os dados do Couchbase - OK
  - Deve ser possível ler os dados pelo IP e Nome da Pagina - OK
  - Deve ser usado N1QL - OK
  - Deve ter os Indices no Couchbase - OK


## Stack de Desenvolvimento
- Dotnet Core 2 
  - (Static File)
  - (WEB API)
  - (Robô)
- RabbitMQ
- Couchbase 4.5 ou Superior
