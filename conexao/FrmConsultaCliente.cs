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
    public partial class FrmConsultaCliente : Form
    {
        FrmCadastroCliente frmCliente;
        public FrmConsultaCliente(FrmCadastroCliente frmCliente)
        {
            this.frmCliente = frmCliente;
            InitializeComponent();
        }

        private void FrmConsultaCliente_Load(object sender, EventArgs e)
        {
            string sqlQuery;
            SqlConnection conCliente = Conexao.getConnection();
            sqlQuery = "SELECT id_cliente,nome,cpf,data_nasc, telefone, cidade, estado, cep, bairro, rua, complemento,numero FROM cliente ORDER BY nome";
            SqlDataAdapter dta = new SqlDataAdapter(sqlQuery, conCliente);
            DataTable dt = new DataTable();
            try
            {
                dta.Fill(dt);
                dgvCliente.DataSource = dt;
                dgvCliente.RowsDefaultCellStyle.BackColor = Color.White;
                dgvCliente.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;

                dgvCliente.Columns[0].HeaderCell.Value = "Codigo do Cliente";
                dgvCliente.Columns[1].HeaderCell.Value = "Nome";
                dgvCliente.Columns[2].HeaderCell.Value = "CPF";
                dgvCliente.Columns[3].HeaderCell.Value = "Dt.Nasc.";
                dgvCliente.Columns[4].HeaderCell.Value = "Telefone";
                dgvCliente.Columns[5].HeaderCell.Value = "Cidade";
                dgvCliente.Columns[6].HeaderCell.Value = "Estado";
                dgvCliente.Columns[7].HeaderCell.Value = "CEP";
                dgvCliente.Columns[8].HeaderCell.Value = "Bairro";
                dgvCliente.Columns[9].HeaderCell.Value = "Rua";
                dgvCliente.Columns[10].HeaderCell.Value = "Complemento";
                dgvCliente.Columns[11].HeaderCell.Value = "Numero";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao listar clientes" + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            //variável para guardar o código do cliente selecionado no DataGridView
            string codigoCliente;

            //seleciona o código da linha atual da coluna[0] do DataGridView
            codigoCliente = dgvCliente.CurrentRow.Cells[0].Value.ToString();

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conClienteConsulta = Conexao.getConnection();

            //declara e inicializa um DataReader como null
            SqlDataReader dtr = null;

            //cria a instrução sql, parametrizada
            sqlQuery = "SELECT id_cliente, nome, cpf, data_nasc, telefone, cidade, estado, cep, bairro, rua , complemento, numero FROM cliente WHERE id_cliente = @id_cliente";

            //tratamento de exceções
            try
            {
                //abrir conexão
                conClienteConsulta.Open();

                // cria um command para esta conexão
                SqlCommand cmd = new SqlCommand(sqlQuery, conClienteConsulta);

                //adiciona um parâmetro
                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(codigoCliente)));

                //Atribui o comando ao DataReader
                dtr = cmd.ExecuteReader();

                //verifica se retornou registro na consulta SQL
                //se retornou, preenche a tela do frmCadastroCliente com os dados armazenados no DataReader dtr
                //chama o método Read( ) para ler os registros no dtr
                if (dtr.Read())
                {
                    //para o Form frmCadastroCliente
                    //atribui ao TextBox o valor do campo ID_CLIENTE do banco de dados retornado na consulta
                    frmCliente.txtCodigo.Text = dtr["ID_CLIENTE"].ToString();

                    frmCliente.txtNome.Text = dtr["NOME"].ToString();
                    frmCliente.mskDtNasc.Text = dtr["DATA_NASC"].ToString();
                    frmCliente.mskCPF.Text = dtr["CPF"].ToString();
                    frmCliente.txtTelefone.Text = dtr["TELEFONE"].ToString();
                    frmCliente.txtCidade.Text = dtr["CIDADE"].ToString();
                    frmCliente.txtEstado.Text = dtr["ESTADO"].ToString();
                    frmCliente.txtCEP.Text = dtr["CEP"].ToString();
                    frmCliente.txtBairro.Text = dtr["BAIRRO"].ToString();
                    frmCliente.txtRua.Text = dtr["RUA"].ToString();
                    frmCliente.txtComplemento.Text = dtr["COMPLEMENTO"].ToString();
                    frmCliente.txtNumero.Text = dtr["NUMERO"].ToString();
                }
            }
            //se houve exceção as instruções do bloco catch serão executadas
            catch (Exception ex)
            {
                //exibe a mensagem sobre a exceção ocorrida no bloco try
                MessageBox.Show(ex.ToString());
            }
            //as instruções de finally sempre serão executadas independente se houve exceção ou não
            finally
            {
                //fecha o DataReader se for diferente de null, ou sej,a se estiver conectado
                if (dtr != null)
                {
                    dtr.Close();
                }
                //fecha a conexão se for diferente de null
                if (conClienteConsulta != null)
                {
                    conClienteConsulta.Close();
                }
            }

            //fecha o form
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
