

using System.Diagnostics.Contracts;
using System.Security;

public class AVL<T> : ABP<T>, IAVL<T> where T : IComparable<T>
{
    public AVL() : base() { }
    public AVL(T dado) : base(dado) { }

    // ======================
    // Sobrescrevendo Insert
    // ======================
    public override void Insert(T dado)
    {
        Raiz = InserirDadoAVL(Raiz, dado, null);
    }

    private Node<T> InserirDadoAVL(Node<T>? noAtual, T dado, Node<T>? pai)
    {
        if (noAtual is null)
        {
            var novo = new Node<T>(dado);
            novo.Pai = pai;
            Tamanho++;

            AtualizaFatorBalanceamentoInserir(pai, novo);

            return novo;
        }

        int comparador = dado.CompareTo(noAtual.Dado);

        if (comparador < 0)
        {
            noAtual.Esquerda = InserirDadoAVL(noAtual.Esquerda, dado, noAtual);
        }
        if (comparador > 0)
        {
            noAtual.Direita = InserirDadoAVL(noAtual.Direita, dado, noAtual);
        }

        return noAtual;

    }


    // ======================
    // Sobrescrevendo Remove
    // ======================


    // ======================
    // Sobrescrevendo Search
    // ======================


    // ======================
    // Rotações AVL
    // ======================

    public void RotacaoDuplaDireita(Node<T> no)
    {

    }

    public void RotacaoDuplaEsquerda(Node<T> no)
    {

    }

    public void RotacaoSimplesDireita(Node<T> no)
    {

    }

    public void RotacaoSimplesEsquerda(Node<T> pai)
    {
        var filhoDireito = pai.Direita;

        if (filhoDireito.Esquerda is not null)
        {


        }
    }

    // ======================
    // Auxiliares AVL
    // ======================
    private void AtualizaFatorBalanceamentoInserir(Node<T>? pai, Node<T> filho)
    {

        while (pai is not null)
        {
            if (pai.Esquerda == filho)
            {
                pai.FatorBalanceamento++;
            }
            else if (pai.Direita == filho)
            {
                pai.FatorBalanceamento--;
            }

            // Checa desbalanceamento
            if (pai.FatorBalanceamento == 2)
            {

            }

            if (pai.FatorBalanceamento == -2)
            {
                if (pai.Direita.FatorBalanceamento == -1)
                {
                    RotacaoSimplesEsquerda(pai);
                }
            }

            filho = pai;
            pai = pai.Pai;
        }

    }

}