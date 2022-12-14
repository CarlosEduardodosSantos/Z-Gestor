namespace ZapFood.WinForm
{
    partial class FromGrupoProduto
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clQuantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnNovo = new System.Windows.Forms.Button();
            this.pnlImagem = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRelacionar = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtLabelGrupo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnAddImageZ = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.panel7.SuspendLayout();
            this.pnlImagem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(784, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cadastro de Grupo de produtos";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 247);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 279);
            this.panel1.TabIndex = 9;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dataGridView3);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(784, 279);
            this.panel6.TabIndex = 2;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AllowUserToResizeColumns = false;
            this.dataGridView3.AllowUserToResizeRows = false;
            this.dataGridView3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView3.ColumnHeadersHeight = 30;
            this.dataGridView3.ColumnHeadersVisible = false;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.clQuantidade});
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(0, 34);
            this.dataGridView3.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(784, 245);
            this.dataGridView3.TabIndex = 0;
            this.dataGridView3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
            this.dataGridView3.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView3_CellMouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CategoriaId";
            this.dataGridViewTextBoxColumn2.FillWeight = 193.6306F;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // clQuantidade
            // 
            this.clQuantidade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clQuantidade.DataPropertyName = "NomeCategoria";
            this.clQuantidade.FillWeight = 6.369431F;
            this.clQuantidade.HeaderText = "Categoria";
            this.clQuantidade.Name = "clQuantidade";
            this.clQuantidade.ReadOnly = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnDown);
            this.panel7.Controls.Add(this.btnUp);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(784, 34);
            this.panel7.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.down32;
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDown.Location = new System.Drawing.Point(745, 6);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 3;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Visible = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.up32;
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUp.Location = new System.Drawing.Point(713, 6);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 2;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Visible = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.Info;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(784, 34);
            this.label11.TabIndex = 0;
            this.label11.Text = "Cadastrados";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.fundoverde;
            this.btnAdicionar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGreen;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdicionar.ForeColor = System.Drawing.Color.White;
            this.btnAdicionar.Location = new System.Drawing.Point(648, 193);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(121, 32);
            this.btnAdicionar.TabIndex = 5;
            this.btnAdicionar.Text = "Salvar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnAddImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnAddImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddImage.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddImage.ForeColor = System.Drawing.Color.White;
            this.btnAddImage.Location = new System.Drawing.Point(12, 193);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(134, 32);
            this.btnAddImage.TabIndex = 16;
            this.btnAddImage.Text = "Alterar Imagem";
            this.btnAddImage.UseVisualStyleBackColor = false;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::ZapFood.WinForm.Properties.Resources.remove_icone_5931_16;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // btnNovo
            // 
            this.btnNovo.BackColor = System.Drawing.Color.Gold;
            this.btnNovo.FlatAppearance.BorderSize = 0;
            this.btnNovo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSeaGreen;
            this.btnNovo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSeaGreen;
            this.btnNovo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovo.Image = global::ZapFood.WinForm.Properties.Resources.add_wirth_32;
            this.btnNovo.Location = new System.Drawing.Point(9, 4);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(35, 32);
            this.btnNovo.TabIndex = 17;
            this.btnNovo.UseVisualStyleBackColor = false;
            this.btnNovo.Visible = false;
            this.btnNovo.Click += new System.EventHandler(this.btnNovo_Click);
            // 
            // pnlImagem
            // 
            this.pnlImagem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlImagem.Controls.Add(this.label6);
            this.pnlImagem.Controls.Add(this.txtNome);
            this.pnlImagem.Controls.Add(this.txtDescricao);
            this.pnlImagem.Controls.Add(this.label3);
            this.pnlImagem.Location = new System.Drawing.Point(305, 50);
            this.pnlImagem.Name = "pnlImagem";
            this.pnlImagem.Size = new System.Drawing.Size(464, 129);
            this.pnlImagem.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(15, 30);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(446, 24);
            this.txtNome.TabIndex = 6;
            this.txtNome.Leave += new System.EventHandler(this.txtNome_Leave);
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(15, 74);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(446, 45);
            this.txtDescricao.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Descrição";
            // 
            // btnRelacionar
            // 
            this.btnRelacionar.BackColor = System.Drawing.Color.Goldenrod;
            this.btnRelacionar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.btnRelacionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelacionar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnRelacionar.ForeColor = System.Drawing.Color.White;
            this.btnRelacionar.Location = new System.Drawing.Point(305, 193);
            this.btnRelacionar.Name = "btnRelacionar";
            this.btnRelacionar.Size = new System.Drawing.Size(201, 32);
            this.btnRelacionar.TabIndex = 18;
            this.btnRelacionar.Text = "Adicionar Categorias";
            this.btnRelacionar.UseVisualStyleBackColor = false;
            this.btnRelacionar.Click += new System.EventHandler(this.btnRelacionar_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.download;
            this.btnExcluir.Enabled = false;
            this.btnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkRed;
            this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluir.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnExcluir.ForeColor = System.Drawing.Color.White;
            this.btnExcluir.Location = new System.Drawing.Point(512, 193);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(130, 32);
            this.btnExcluir.TabIndex = 19;
            this.btnExcluir.Text = "Excluir";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // txtLabelGrupo
            // 
            this.txtLabelGrupo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(25)))), ((int)(((byte)(0)))));
            this.txtLabelGrupo.ForeColor = System.Drawing.Color.White;
            this.txtLabelGrupo.Location = new System.Drawing.Point(12, 156);
            this.txtLabelGrupo.Name = "txtLabelGrupo";
            this.txtLabelGrupo.Size = new System.Drawing.Size(134, 19);
            this.txtLabelGrupo.TabIndex = 22;
            this.txtLabelGrupo.Text = "Imagem App ";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(25)))), ((int)(((byte)(0)))));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(158, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Imagem Zimmer";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(158, 50);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(134, 129);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 23;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // btnAddImageZ
            // 
            this.btnAddImageZ.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnAddImageZ.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnAddImageZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddImageZ.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddImageZ.ForeColor = System.Drawing.Color.White;
            this.btnAddImageZ.Location = new System.Drawing.Point(158, 193);
            this.btnAddImageZ.Name = "btnAddImageZ";
            this.btnAddImageZ.Size = new System.Drawing.Size(134, 32);
            this.btnAddImageZ.TabIndex = 25;
            this.btnAddImageZ.Text = "Alterar Imagem";
            this.btnAddImageZ.UseVisualStyleBackColor = false;
            this.btnAddImageZ.Click += new System.EventHandler(this.button1_Click);
            // 
            // FromGrupoProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 526);
            this.Controls.Add(this.btnAddImageZ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtLabelGrupo);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.btnRelacionar);
            this.Controls.Add(this.btnNovo);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.pnlImagem);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Name = "FromGrupoProduto";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo de produtos";
            this.Load += new System.EventHandler(this.FormTipoOpcoes_Load);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.panel7.ResumeLayout(false);
            this.pnlImagem.ResumeLayout(false);
            this.pnlImagem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Panel pnlImagem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.Button btnNovo;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button btnRelacionar;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clQuantidade;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label txtLabelGrupo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnAddImageZ;
    }
}