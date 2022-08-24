namespace ZapFood.WinForm
{
    partial class FormPesquisaProduto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.clProdutoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clVlVenda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clValorRegular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clProdutoId,
            this.clProduto,
            this.clVlVenda,
            this.clValorRegular});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(584, 441);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // clProdutoId
            // 
            this.clProdutoId.DataPropertyName = "referenciaId";
            this.clProdutoId.Frozen = true;
            this.clProdutoId.HeaderText = "Código";
            this.clProdutoId.Name = "clProdutoId";
            this.clProdutoId.ReadOnly = true;
            this.clProdutoId.Width = 80;
            // 
            // clProduto
            // 
            this.clProduto.DataPropertyName = "nome";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clProduto.DefaultCellStyle = dataGridViewCellStyle1;
            this.clProduto.Frozen = true;
            this.clProduto.HeaderText = "Produto";
            this.clProduto.Name = "clProduto";
            this.clProduto.ReadOnly = true;
            this.clProduto.Width = 280;
            // 
            // clVlVenda
            // 
            this.clVlVenda.DataPropertyName = "valorVenda";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clVlVenda.DefaultCellStyle = dataGridViewCellStyle2;
            this.clVlVenda.Frozen = true;
            this.clVlVenda.HeaderText = "Valor Venda";
            this.clVlVenda.Name = "clVlVenda";
            this.clVlVenda.ReadOnly = true;
            // 
            // clValorRegular
            // 
            this.clValorRegular.DataPropertyName = "valorRegular";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.clValorRegular.DefaultCellStyle = dataGridViewCellStyle3;
            this.clValorRegular.Frozen = true;
            this.clValorRegular.HeaderText = "Valor Regular";
            this.clValorRegular.Name = "clValorRegular";
            this.clValorRegular.ReadOnly = true;
            // 
            // FormPesquisaProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "FormPesquisaProduto";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pesquisa do produto";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clProdutoId;
        private System.Windows.Forms.DataGridViewTextBoxColumn clProduto;
        private System.Windows.Forms.DataGridViewTextBoxColumn clVlVenda;
        private System.Windows.Forms.DataGridViewTextBoxColumn clValorRegular;
    }
}