# Árvore AVL - Implementação em C#

Este repositório contém uma implementação completa de uma Árvore AVL em C#, desenvolvida como parte de uma atividade acadêmica.

## 📋 Descrição

A Árvore AVL (Adelson-Velsky e Landis) é uma estrutura de dados de árvore binária de busca auto-balanceada. Esta implementação em C# garante que todas as operações básicas (inserção, remoção e busca) sejam executadas em tempo O(log n), mantendo o balanceamento através de rotações.

## 🚀 Funcionalidades

- **Inserção**: Adição de nós mantendo o balanceamento
- **Remoção**: Exclusão de nós com rebalanceamento automático
- **Busca**: Pesquisa eficiente de elementos
- **Exibição**: Visualização da árvore de forma hierárquica
- **Herança**: Implementação que herda de uma Árvore Binária de Pesquisa (ABP)

## 🛠️ Tecnologias

- **Linguagem**: C# (.NET 8.0)
- **Paradigma**: Orientação a Objetos
- **Arquitetura**: Separação em interfaces e implementações
- **Recursos**: Herança, recursividade, balanceamento automático

## 📊 Estrutura do Projeto

```
ArvoreAVL/
├── Entities/
│   ├── ABP.cs          # Classe base Árvore Binária de Pesquisa
│   ├── AVL.cs          # Classe AVL que herda de ABP
│   └── Node.cs         # Classe que representa um nó da árvore
├── Interfaces/
│   ├── IABP.cs         # Interface para a ABP
│   └── IAVL.cs         # Interface para a AVL
└── Program.cs          # Classe principal para testes
```

## 🎯 Como Executar

1. Certifique-se de ter o [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) instalado

2. Clone o repositório:
```bash
git clone https://github.com/IsaacLira42/ArvoreAVL.git
```

3. Navegue até o diretório do projeto:
```bash
cd ArvoreAVL/ArvoreAVL
```

4. Compile e execute o projeto:
```bash
dotnet run
```

## 📝 Operações Disponíveis

O programa principal oferece um menu interativo com as seguintes opções:

1. **Inserir nó** - Adiciona um novo valor à árvore
2. **Remover nó** - Remove um valor específico
3. **Buscar nó** - Verifica se um valor existe na árvore
4. **Mostrar árvore** - Exibe a estrutura hierárquica
5. **Sair** - Encerra o programa

## ⚖️ Balanceamento

A implementação utiliza as quatro rotações clássicas da AVL:
- Rotação simples à direita
- Rotação simples à esquerda
- Rotação dupla direita-esquerda
- Rotação dupla esquerda-direita

O fator de balanceamento é recalculado após cada operação usando as fórmulas vistas em aula.

## 📈 Complexidade

Todas as operações garantem complexidade O(log n):
- Inserção: O(log n)
- Remoção: O(log n)
- Busca: O(log n)

## 📄 Documentação

O projeto inclui um arquivo DOC com o código completo, conforme exigido na especificação da atividade.

## 👨‍💻 Desenvolvido por

[Isaac Lira](https://github.com/IsaacLira42) - Como parte das atividades acadêmicas de Estruturas de Dados
