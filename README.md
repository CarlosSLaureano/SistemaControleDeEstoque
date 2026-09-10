# 🛒 Gestor360 - Sistema de Controle de Estoque em ASP.NET Core MVC

Sistema completo e moderno de controle de estoque desenvolvido em **.NET 10** e **SQL Server**, estruturado com o padrão de arquitetura **MVC (Model-View-Controller)** e princípios de código limpo.

O projeto traz uma interface responsiva e elegante com conceitos de *Glassmorphism*, feedback interativo e foco em usabilidade para ambientes corporativos.

---

## 🔑 Principais Funcionalidades

- 📊 **Dashboard Interativo**: Painel de visão geral com métricas essenciais, alertas de estoque baixo e atalhos operacionais rápidos.
- 📦 **Gestão de Produtos e Categorias**: Controle de itens, categorias, preços de compra/venda, quantidade em estoque e movimentações.
- 👥 **Cadastro de Clientes**: Registro completo de clientes com atalho para contato direto via WhatsApp.
- 👤 **Controle de Acesso e Perfis (ACL)**:
  - **Administrador**: Gestão total do sistema, incluindo usuários e auditoria.
  - **Padrão**: Acesso restrito às operações do dia a dia.
- 🔒 **Segurança e Autenticação**:
  - Criptografia de senhas com *BCrypt*.
  - Redefinição e alteração segura de senhas.
  - Filtros de sessão e autorização personalizada por rota.
- 📑 **Exportação e Relatórios**:
  - Exportação de dados filtrados para planilhas **Excel (.xlsx)** via *ClosedXML*.
  - Geração de relatórios em **PDF** via *QuestPDF*.
- 📝 **Auditoria e Logs de Atividades**: Rastreamento detalhado das ações realizadas pelos usuários (acessível por administradores).
- 💬 **Experiência do Usuário (UX/UI)**:
  - Confirmações de ações críticas via *SweetAlert2*.
  - Tabelas interativas com ordenação, busca e paginação via *DataTables*.
  - *Empty states* amigáveis e validações em tempo real.

---

## 🚀 Tecnologias e Bibliotecas

### Back-end & Testes
- **Framework:** .NET 10 (C#)
- **Acesso a Dados:** Entity Framework Core & SQL Server
- **Criptografia:** BCrypt.Net-Next
- **Relatórios:** ClosedXML (Excel) & QuestPDF (PDF)
- **Serialização:** Newtonsoft.Json
- **Testes Automatizados:** xUnit, Moq, EF Core InMemory, Coverlet

### Front-end
- **Tecnologias:** HTML5, CSS3 avançado (Glassmorphism & Flexbox/Grid), JavaScript
- **Framework & Componentes:** Bootstrap 5, FontAwesome, DataTables, SweetAlert2, jQuery

---

## 📁 Estrutura do Projeto

```text
SistemaControleDeEstoque/
├── SistemaControleEstoque/              # Aplicação Web MVC (.NET 10)
│   ├── Controllers/                    # Controladores da aplicação
│   ├── Data/                           # DbContext e mapeamento
│   ├── Filters/                        # Filtros de sessão e autorização
│   ├── Helper/                         # Utilitários, sessão e criptografia
│   ├── Migrations/                     # Migrações do Entity Framework
│   ├── Models/                         # Modelos de domínio e ViewModels
│   ├── Repositorio/                    # Camada de repositório e regras de acesso
│   ├── Views/                          # Telas Razor (Views e Componentes)
│   └── wwwroot/                        # Arquivos estáticos (CSS, JS, Imagens)
│
├── SistemaControleEstoque.Tests/        # Projeto de Testes Unitários (.NET 10)
│   ├── Controllers/                    # Testes de controladores
│   └── Repositorios/                   # Testes de repositórios
│
└── README.md
```

---

## ⚙️ Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) ou LocalDB
- Ferramenta `dotnet-ef` (opcional para rodar migrações manualmente):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 🛠️ Como Executar o Projeto

### 1. Clonar o repositório
```bash
git clone https://github.com/CarlosSLaureano/SistemaControleDeEstoque.git
cd SistemaControleDeEstoque
```

### 2. Configurar a String de Conexão
Abra o arquivo [`SistemaControleEstoque/appsettings.json`](file:///c:/Dev/SistemaControleDeEstoque/SistemaControleEstoque/appsettings.json) e ajuste a conexão com o seu SQL Server:
```json
"ConnectionStrings": {
  "DataBase": "server=SEU_SERVIDOR;database=DB_ControleEstoque;trusted_connection=true;trustservercertificate=true"
}
```

### 3. Aplicar as Migrações do Banco de Dados
```bash
dotnet ef database update --project ./SistemaControleEstoque/SistemaControleEstoque.csproj
```

### 4. Executar a Aplicação
```bash
dotnet run --project ./SistemaControleEstoque/SistemaControleEstoque.csproj
```
Abra o navegador no endereço indicado (por exemplo `https://localhost:7001` ou `http://localhost:5000`).

---

## 🧪 Como Executar os Testes

Para rodar a suíte de testes unitários com xUnit:
```bash
dotnet test ./SistemaControleEstoque.Tests/SistemaControleEstoque.Tests.csproj
```

---

## 📄 Licença

Este projeto é desenvolvido para fins de controle e gestão de estoque. Consulte o repositório para detalhes sobre direitos e contribuições.
