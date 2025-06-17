# PedidoConsumer

O **PedidoConsumer** é um serviço em .NET que funciona como consumidor de mensagens publicadas em uma fila RabbitMQ. Ele processa os dados recebidos e os armazena em um banco de dados **SQL Server** utilizando o **Entity Framework Core**.

Este projeto faz parte de um sistema de mensageria onde a **ApiPublisher** publica pedidos e o **PedidoConsumer** os consome.

---

## Tecnologias Utilizadas

- .NET 8 (Worker Service)
- RabbitMQ
- Entity Framework Core
- SQL Server
- C#
- Docker (RabbitMQ, SQL Server)
