using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace conexao
{
    public partial class FrmCadastroCliente : Form
    {
        public FrmCadastroCliente()
        {
            InitializeComponent();
        }
        private void habilitar()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = true;
            mskCPF.Enabled = true;
            mskDtNasc.Enabled = true;
            txtTelefone.Enabled = true;
            txtCidade.Enabled = true;
            txtEstado.Enabled = true;
            txtCEP.Enabled = true;
            txtBairro.Enabled = true;
            
            txtComplemento.Enabled = true;
            txtNumero.Enabled = true;
            txtRua.Enabled = true;
        }
        private void desHabilitar()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            mskCPF.Enabled = false;
            mskDtNasc.Enabled = false;
            txtTelefone.Enabled = false;
            txtCidade.Enabled = false;
            txtEstado.Enabled = false;
            txtCEP.Enabled = false;
            txtBairro.Enabled = false;
            txtRua.Enabled = false;
            txtComplemento.Enabled = false;
            txtNumero.Enabled = false;
        }
        private void limparControles()
        {
            txtCodigo.Enabled = false;

            txtCodigo.Clear();
            txtNome.Clear();
            mskCPF.Clear();
            mskDtNasc.Clear();
            mskCPF.Focus();
            txtTelefone.Clear();
            txtCidade.Clear();
            txtEstado.Clear();
            txtCEP.Clear();
            txtBairro.Clear();
            txtRua.Clear();
            txtComplemento.Clear();
            txtNumero.Clear();
        }

        private bool validaDados()
        {
            if (string.IsNullOrEmpty(mskCPF.Text))
            {
                MessageBox.Show("Preenchimento de campo obrigatorio", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mskCPF.Clear();
                mskCPF.Focus();

                return false;

            }
            return true;
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (MessageBox.Show("Voce esta editando um registro existente.Deseja incluir um registro novo?",
                   "ACR Rental Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    limparControles();
                return;
            }
            if (validaDados() == false)
                return;
            string sqlQuery;
            SqlConnection conCliente = Conexao.getConnection();
            sqlQuery = "INSERT INTO cliente(nome,data_nasc,cpf,telefone,cep,cidade,estado,bairro,rua,complemento,numero) VALUES (@nome,@data_nasc,@cpf,@telefone,@cep,@cidade,@estado,@bairro,@rua,@complemento,@numero)";
            try
            {
                conCliente.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);
                cmd.Parameters.Add(new SqlParameter("@nome", txtNome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(mskDtNasc.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", mskCPF.Text));
                cmd.Parameters.Add(new SqlParameter("@telefone", txtTelefone.Text));
                cmd.Parameters.Add(new SqlParameter("@cep", txtCEP.Text));
                cmd.Parameters.Add(new SqlParameter("@cidade", txtCidade.Text));
                cmd.Parameters.Add(new SqlParameter("@estado", txtEstado.Text));

                cmd.Parameters.Add(new SqlParameter("@bairro", txtBairro.Text));
                cmd.Parameters.Add(new SqlParameter("@rua", txtRua.Text));
                cmd.Parameters.Add(new SqlParameter("@complemento", txtComplemento.Text));
                cmd.Parameters.Add(new SqlParameter("@numero", txtNumero.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cliente incluido com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limparControles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao incluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                { conCliente.Close(); }

            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void FrmCadastroClientes_Load(object sender, EventArgs e)
        {
            habilitar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Form frm = new FrmConsultaCliente(this);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void mskCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            //os campos para serem alterados são preenchidos por meio da consulta no grid do form Consulta de Cliente
            //verifica se tem o código do cliente no txtCodigo. Se o campo estiver vazio, interrompe a sub-rotina
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja alterar clicando no botão consultar", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;   //interrompe a sub-rotina
            }

            // antes de alterar o registro é preciso validar os dados de preenchimento obrigatório
            //chama o método para validar a entrada de dados
            //se retornou falso, interrompe o processamento
            if (validaDados() == false)
            {
                return;
            }

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conCliente = Conexao.getConnection();

            //cria a instrução sql, parametrizada
            sqlQuery = "UPDATE cliente set nome=@nome,data_nasc=@data_nasc,cpf=@cpf,telefone=@telefone,cep=@cep ,cidade=@cidade,estado=@estado,bairro=@bairro,rua=@rua,complemento=@complemento,numero=@numero WHERE id_cliente=@id_cliente";

            //Tratamento de exceções 
            //códigos semelhantes ao botão inserir com diferença na instrução SQL
            try
            {
                conCliente.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);
                //define, adiciona os parametros
                cmd.Parameters.Add(new SqlParameter("@nome", txtNome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(mskDtNasc.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", mskCPF.Text));
                cmd.Parameters.Add(new SqlParameter("@telefone", txtTelefone.Text));
                cmd.Parameters.Add(new SqlParameter("@cep", txtCEP.Text));
                cmd.Parameters.Add(new SqlParameter("@cidade", txtCidade.Text));
                cmd.Parameters.Add(new SqlParameter("@estado", txtEstado.Text));
                cmd.Parameters.Add(new SqlParameter("@bairro", txtBairro.Text));
                cmd.Parameters.Add(new SqlParameter("@rua", txtRua.Text));
                cmd.Parameters.Add(new SqlParameter("@complemento", txtComplemento.Text));
                cmd.Parameters.Add(new SqlParameter("@numero", txtNumero.Text));
                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txtCodigo.Text)));

                //executa o comando
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente alterado com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpa os campos para nova entrada de dados
                limparControles();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao alterar cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //veririfica se tem o código do cliente no txtCodigo
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja excluir clicando no botão consultar", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //solicita confirmação de exclusão de registro
            if (MessageBox.Show("Deseja excluir permanentemente o registro?", "ACR Rental Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //declaração da variável para guardar as instruções SQL
                string sqlQuery;

                //cria conexão chamando o método getConnection da classe Conexao
                SqlConnection conCliente = Conexao.getConnection();

                //cria a instrução sql, parametrizada
                sqlQuery = "DELETE FROM cliente WHERE id_cliente=@id_cliente";

                //Tratamento de exceções
                try
                {
                    conCliente.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                    //define, adiciona os parametros
                    cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txtCodigo.Text)));

                    //executa o commando
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente excluído com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Limpa os campos para nova entrada de dados
                    limparControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problema ao incluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    if (conCliente != null)
                    {
                        conCliente.Close();
                    }
                }
            }
        }

        private void FrmCadastroCliente_Load(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblDtNasc_Click(object sender, EventArgs e)
        {

        }

        private void lblCep_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCEP.Text))
            {
                using (var ws = new WSCorreios.AtendeClienteClient())
                {
                    try
                    {
                        var endereco = ws.consultaCEP(txtCEP.Text.Trim());

                        txtEstado.Text = endereco.uf;
                        txtCidade.Text = endereco.cidade;
                        txtBairro.Text = endereco.bairro;
                        txtRua.Text = endereco.end;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Informe um CEP válido...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCEP.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtRua.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtNumero.Text = string.Empty;

            txtNome.Text = string.Empty;
            mskCPF.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            mskDtNasc.Text = string.Empty;
            txtCodigo.Text = string.Empty;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lblNome_Click(object sender, EventArgs e)
        {

        }

        private void lblCPF_Click(object sender, EventArgs e)
        {

        }

        private void mskDtNasc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void lblTelefone_Click(object sender, EventArgs e)
        {

        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCodigo_Click(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCEP_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }
    
    

