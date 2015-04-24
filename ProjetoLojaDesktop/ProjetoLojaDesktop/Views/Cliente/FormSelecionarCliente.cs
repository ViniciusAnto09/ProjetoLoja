using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProjetoLojaDesktop.Entity;
using ProjetoLojaDesktop.Data;

namespace ProjetoLojaDesktop.Views.Cliente
{
    public partial class FormSelecionarCliente : Form
    {
        private Pessoa pessoa;
        private PessoaData pessoaData;
        private PessoaFisicaData pessoaFisicaData;
        private PessoaJuridicaData pessoaJuridicaData;

        public FormSelecionarCliente()
        {
            InitializeComponent();
            ProjetoLojaEntities db = new ProjetoLojaEntities();
            pessoaData = new PessoaData(db);
            pessoaFisicaData = new PessoaFisicaData(db);
            pessoaJuridicaData = new PessoaJuridicaData(db);


            atualizarListaClientes(pessoaData.todasPessoas());
        }

        public void atualizarListaClientes(List<Pessoa> pessoas)
        {
            var lista = from p in pessoas
                        join pf in pessoaFisicaData.todasPessoaFisicas()
                            on p.idPessoa equals pf.idPessoa
                        join pj in pessoaJuridicaData.todasPessoasJuridicas()
                            on p.idPessoa equals pj.idPessoa
                        select new
                        {
                            Pessoa = p,
                            Email = p.email,
                            CPF = pf.CPF,
                            CNPJ = pj.CNPJ
                        };

            dgvClientes.DataSource = pessoas;
            dgvClientes.Columns[0].Visible = false;
            dgvClientes.Columns[1].HeaderText = "Nome";
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            atualizarListaClientes(pessoaData.pesquisarPessoaPorNome(txtPesquisa.Text));
        }

        private Pessoa getClienteSelecionadoNaTabela()
        {
            DataGridViewRow p = dgvClientes.CurrentRow;
            if (p != null)
                return (Pessoa)p.DataBoundItem;

            return null;
        }

    }
}
