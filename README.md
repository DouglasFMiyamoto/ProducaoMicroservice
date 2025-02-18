# Microserviço de Produção

## Descrição
O microserviço de Produção é responsável por iniciar a produção dos pedidos que tiveram o pagamento confirmado. Ele consome mensagens da fila `pedido-pago` e, ao final do processo de produção, publica uma mensagem na fila `pedido-atualizado` informando a conclusão ou mudança de status do pedido.

## Fluxo da Produção
1. **Consumo da Fila `pedido-pago`**: O serviço consome mensagens da fila Amazon SQS `pedido-pago`.
2. **Início da Produção**: O pedido é enviado para o processo de produção.
3. **Atualização do Status**: Conforme avança, o status do pedido é atualizado.
4. **Publicação na Fila `pedido-atualizado`**: Após a conclusão da produção ou qualquer alteração relevante, o serviço publica uma mensagem na fila `pedido-atualizado`.

## Tecnologias Utilizadas
- **.NET 8** (C#)
- **Amazon SQS** (Mensageria)
- **Docker** (Containerização)
- **LocalStack** (Simulação do AWS SQS em ambiente local para testes)

## Como Executar o Microserviço
### 1. Configurar Dependências
Certifique-se de que você tem os seguintes serviços configurados e em execução:
- **LocalStack (opcional para testes locais)**: Para simular o SQS.

### 2. Configurar Variáveis de Ambiente
Defina as variáveis necessárias no ambiente para conexão com o SQS.

Exemplo de configuração no `appsettings.json`:
```json
{
  "AWS": {
    "SQS": {
      "PedidoPagoQueueUrl": "http://localhost:4566/000000000000/pedido-pago",
      "PedidoAtualizadoQueueUrl": "http://localhost:4566/000000000000/pedido-atualizado"
    }
  }
}
```

### 3. Executar a Aplicação
Com o Docker:
```sh
docker-compose up --build
```
Ou localmente:
```sh
dotnet run
```

## Testes
Este microserviço conta com testes de unidade para garantir a qualidade do código. Para rodar os testes:
```sh
dotnet test
```
