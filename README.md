# Projeto 78343-24 CLOUD

## Sobre o Projeto

Projeto final da Academia de Desenvolvimento de Software da Rumos, proposto pelo formador <a href="https://github.com/sergiocpxfontes"> Sérgio Fontes </a>.
Consiste em um E-Commerce de venda de produtos de botânica, com área cliente, área administrativa, comunidade do site e ainda área de desafio.

Os clientes podem realizar a compra de plantas, sementes, árvores, entre outras coisas relacionadas a botânica. Podem ainda verificar seus pedidos, cupões disponíveis, posts realizados 
e participar junto à comunidade, apresentando dicas, tirando dúvidas ou mostrando a evolução de suas plantas.

Há ainda a área do desafio do dia, onde os desafios podem ser pré-cadastrados, e são apresentado todos os dias, consoante a data para publicação.
Neste desafio, caso a resposta esteja correta, o utilizador ganhará um cupão de desconto no percentual cadastrado no desafio, pelo administrador.
Podem ser realizadas quantas tentativas forem necessárias até atingir o objetivo de receber o cupão. 
O utilizados pode ter somente um cupão ativo e somente receber um cupão por desafio.

Área administrativa:
Gerenciar produtos, categorias, pedidos, comunidade e Desafio do dia.

O Projeto utiliza a arquitetura de três camadas:
- App - Interface do utilizador.
- Business - Lógica de negócios.
- Data - Acesso à dados.

Tem como objetivo a utilização de recursos da plataforma azure. Assim, foram utilizadas as seguintes funcionalidades:

- Azure SQL DataBase (para a Data Base)
- Azure WebApp (para armazenar a página web)
- Azure KeyVault (para proteger a connection String da Data Base)
- Azure BlobStorage (para armazenar as imagens)
- Pipeline na plataforma Azure Devops (para o CI/CD)
- Docker Image (para a entrega em container)

O Projeto foi desenvolvido no Visual Studio 2022, com .net 8.0.

A autenticação foi feita com o auxílio do Identity da Microsoft, com as devidas alterações para o modelo desejado, possibilitando login e registo de utilizadores.
Para fins de projeto de estudo, o User Admin foi criado com seed durante do desenvolvimento e encontra-se na base de dados com o seguinte login: admin@localhost.com e senha: Admin123*

Está disponível provisóriamente e para fins de conferência no link: (contactar)
A pipeline foi desenvolvida na plataforma AzureDevops.

Este projeto pode ser baixado e utilizado localmente, com a configuração de uma database SQLServer e com a adaptação dos métodos de UploadImage das controllers Challenges, Products e Posts para um repositório local ou outro repositório online.

### Models:
- Address
- UserApplication
- Product
- Category
- OrderItem
- Order
- Post
- Comment
- Challenge
- Coupon

### Algumas Images do Site

#### Gerir Produtos 
![Gerir Produtos](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/gerir-produtos.jpg)
#### Gerir Pedidos 
![Gerir Pedidos](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/gerir-pedidos.jpg)
#### Carrinho de compras 
![Carrinho de Compras](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/carrinho.jpg)
#### Comunidade 
![Comunidade](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/comunidade.jpg)
#### Desafio do Dia 
![Desafio do Dia](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/desafio-dia.jpg)
#### Detalhes do Produto 
![Detalhes do Produto](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/detalhes-produto.jpg)
#### Detalhes do Pedido
![Detalhes do Pedido](https://github.com/camilagbrito/Project_78343-24-CLOUD/blob/main/img/detalhes-pedido.jpg)
