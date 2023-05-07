using System;
using System.Windows.Forms;
using nature_prhysm_launcher;
using nature_prhysm_launcher.Properties;
using NAudio.Wave;

class Program
{
    [STAThread]
    static void Main()
    {
        System.IO.Directory.SetCurrentDirectory(@"../../");
        var form = new MainForm();
        Application.Run(form);
    }
}

class MainForm : Form
{
    SettingSaveLoad settingSaveLoad = new SettingSaveLoad();

    AsioOut asioOut;
    WasapiOut wasapiOut;
    DirectSoundOut directSoundOut;
    
    TabControl tabControl;
    Button gameStartButton = new Button();
    Button resetButton = new Button();
    Button transferSaveDataButton = new Button();

    TabPage tabPage1;
    GroupBox graphicPanel = new GroupBox();
    GroupBox soundPanel = new GroupBox();
    CheckBox vsyncCheckBox = new CheckBox();
    NumericUpDown fpsNumericUpDown = new NumericUpDown();
    CheckBox showFpsCheckBox = new CheckBox();
    ComboBox soundOutputTypeComboBox = new ComboBox();
    ComboBox asioDriverComboBox = new ComboBox();
    CheckBox wasapiExclusiveCheckBox = new CheckBox();
    Label asioDriverLabel = new Label();
    Label bufferLabel = new Label();
    NumericUpDown bufferNumericUpDown = new NumericUpDown();

    TabPage tabPage2;
    CheckBox vsyncOffsetCompensationCheckBox = new CheckBox();
    CheckBox showStrShadowCheckBox = new CheckBox();
    CheckBox useHiperformanceTimerCheckBox = new CheckBox();
    NumericUpDown songSelectRowNumberNumericUpDown = new NumericUpDown();
    NumericUpDown displayTimingOffsetNumericUpDown = new NumericUpDown();
    CheckBox fullScreenCheckBox = new CheckBox();

    TabPage tabPage3;
    CheckBox showDebugCheckBox = new CheckBox();
    CheckBox localCheckBox = new CheckBox();
    CheckBox usePyCheckBox = new CheckBox();




    // �R���X�g���N�^
    public MainForm()
    {
        InitForm();
        InitTabControl();
        InitTabPage1();
        InitTabPage2();
        InitTabPage3();
        InitButton();
        loadSetting();
    }

    private void loadSetting()
    {
        var setting = settingSaveLoad.Load();
        updateSetting(setting);
    }

    private void updateSetting(Setting setting)
    {
        vsyncCheckBox.Checked                       = setting.vsync;
        fpsNumericUpDown.Value                      = inRangeNumericUpDown(setting.fps, fpsNumericUpDown);
        showFpsCheckBox.Checked                     = setting.showFps;
        soundOutputTypeComboBox.SelectedIndex       = inRangeComboBox(setting.soundOutputType, soundOutputTypeComboBox);
        wasapiExclusiveCheckBox.Checked             = setting.wasapiExclusive;
        asioDriverComboBox.SelectedIndex            = inRangeComboBox(setting.asioDriver, asioDriverComboBox);
        bufferNumericUpDown.Value                   = inRangeNumericUpDown(setting.buffer, bufferNumericUpDown);
        vsyncOffsetCompensationCheckBox.Checked     = setting.vsyncOffsetCompensation;
        showStrShadowCheckBox.Checked               = setting.showStrShadow;
        useHiperformanceTimerCheckBox.Checked       = setting.useHiperformanceTimer;
        songSelectRowNumberNumericUpDown.Value      = inRangeNumericUpDown(setting.songSelectRowNumber, songSelectRowNumberNumericUpDown);
        displayTimingOffsetNumericUpDown.Value      = inRangeNumericUpDown(setting.displayTimingOffset, displayTimingOffsetNumericUpDown);
        fullScreenCheckBox.Checked                  = setting.fullScreen;
        showDebugCheckBox.Checked                   = setting.showDebug;
        localCheckBox.Checked                       = setting.local;
        usePyCheckBox.Checked                       = setting.usePy;

        int inRangeNumericUpDown(int value, NumericUpDown numericUpDown)
        {
            if (value >= numericUpDown.Maximum) return (int)numericUpDown.Maximum;
            else if (value < numericUpDown.Minimum) return (int)numericUpDown.Minimum;
            else return value;
        }

        int inRangeComboBox(int value, ComboBox comboBox)
        {
            if (value >= comboBox.Items.Count) return -1;
            else if (value < -1) return (int)-1;
            else return value;
        }
    }

    private Setting makeSetting()
    {
        var setting = new Setting();

        setting.vsync = vsyncCheckBox.Checked;
        setting.fps = (int)fpsNumericUpDown.Value;
        setting.showFps = showFpsCheckBox.Checked;
        setting.soundOutputType = soundOutputTypeComboBox.SelectedIndex;
        setting.wasapiExclusive = wasapiExclusiveCheckBox.Checked;
        setting.asioDriver = asioDriverComboBox.SelectedIndex;
        setting.buffer = (int)bufferNumericUpDown.Value;
        setting.vsyncOffsetCompensation = vsyncOffsetCompensationCheckBox.Checked;
        setting.showStrShadow = showStrShadowCheckBox.Checked;
        setting.useHiperformanceTimer = useHiperformanceTimerCheckBox.Checked;
        setting.songSelectRowNumber = (int)songSelectRowNumberNumericUpDown.Value;
        setting.displayTimingOffset = (int)displayTimingOffsetNumericUpDown.Value;
        setting.fullScreen = fullScreenCheckBox.Checked;
        setting.showDebug = showDebugCheckBox.Checked;
        setting.local = localCheckBox.Checked;
        setting.usePy = usePyCheckBox.Checked;

        return setting;
    }



    private void InitButton()
    {
        gameStartButton.Location = new Point(10, 320);
        gameStartButton.AutoSize = true;
        gameStartButton.Text = "�Q�[���X�^�[�g";
        gameStartButton.Click += new EventHandler(onClickGameStart);
        this.Controls.Add(gameStartButton);

        resetButton.Location = new Point(513, 320);
        resetButton.AutoSize = true;
        resetButton.Text = "�f�t�H���g�ݒ�ɖ߂�";
        resetButton.Click += new EventHandler(onClickReset);
        this.Controls.Add(resetButton);

        transferSaveDataButton.Location = new Point(310, 320);
        transferSaveDataButton.AutoSize = true;
        transferSaveDataButton.Text = "�Z�[�u�f�[�^�̈��p��(ver1.30�ȍ~)";
        transferSaveDataButton.Click += new EventHandler(onClickTransferSaveData);
        this.Controls.Add(transferSaveDataButton);
    }

    private void onClickGameStart(object? sender, EventArgs e)
    {
        //�ݒ�̕ۑ�
        var isSucceed = settingSaveLoad.Save(makeSetting());

        if (isSucceed)
        {
            //�Q�[���̋N��
            System.IO.Directory.SetCurrentDirectory(@"programs/application");

            System.Diagnostics.Process.Start(@"nature_prhysm.exe");
            this.Close();
        }

    }

    private void onClickReset(object? sender, EventArgs e)
    {
        //�ݒ菉�����̊m�F
        DialogResult result = MessageBox.Show(
           "�ݒ�l���f�t�H���g�ɖ߂��܂����H", "�f�t�H���g�ݒ�ɖ߂�",
           MessageBoxButtons.YesNo,    // �{�^���̐ݒ�
           MessageBoxIcon.Question);   // �A�C�R���̐ݒ�

        if (result == DialogResult.Yes)
        {
            var setting = settingSaveLoad.getInitSetting();
            updateSetting(setting);

            MessageBox.Show(
          "�ݒ�l���f�t�H���g�ɖ߂��܂����B", "�f�t�H���g�ݒ�ɖ߂�",
          MessageBoxButtons.OK,    // �{�^���̐ݒ�
          MessageBoxIcon.Information);   // �A�C�R���̐ݒ�
        }

        //�ݒ�̏�����
    }

    private void onClickTransferSaveData(object? sender, EventArgs e)
    {
        SaveDataTransfer saveDataTransfer = new SaveDataTransfer();


        var result = MessageBox.Show(
           "�Z�[�u�f�[�^�������p������nature prhysm�̃t�H���_��I�����Ă�������(ver1.30�ȍ~)�B\n�ړ���̃f�[�^�͍폜����܂��B", "�Z�[�u�f�[�^���p��",
           MessageBoxButtons.OKCancel,    // �{�^���̐ݒ�
           MessageBoxIcon.Information);   // �A�C�R���̐ݒ�

        if (result == DialogResult.Cancel) return;

        FolderBrowserDialog dialog = new FolderBrowserDialog()
        {
            SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),  // �I�������t�H���_�̏����l
            RootFolder = Environment.SpecialFolder.Desktop,  // ���[�g
            Description = "�Z�[�u�f�[�^�������p������nature prhysm�̃t�H���_��I�����Ă��������B",   // ������
        };

        result = dialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            if(System.Environment.CurrentDirectory == dialog.SelectedPath)//���g�Ɠ����t�H���_�͑I�΂��Ȃ�
            {
                MessageBox.Show("���p����Ɠ����t�H���_�͑I�ׂ܂���B���̃t�H���_��I�����Ă��������B", "�Z�[�u�f�[�^���p��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var isSucceed = saveDataTransfer.transfer(dialog.SelectedPath);
            if (isSucceed) loadSetting();
        }

        //�͂���I�񂾂�Z�[�u�f�[�^�̈��p������
    }


    private void InitTabPage1()
    {
        tabPage1 = new TabPage();
        tabPage1.Name = "tab1";
        tabPage1.Text = "��{�ݒ�";
        tabControl.TabPages.Add(tabPage1);

        graphicPanel.Text = "�O���t�B�b�N";
        graphicPanel.Location = new Point(10, 0);
        graphicPanel.Size = new Size(590,145);
        tabPage1.Controls.Add(graphicPanel);

        soundPanel.Text = "�T�E���h";
//        soundPanel.BorderStyle = BorderStyle.FixedSingle;
        soundPanel.Location = new Point(10, 150);
        soundPanel.Size = new Size(590, 110);
        tabPage1.Controls.Add(soundPanel);

        vsyncCheckBox.Text = "����������ON�ɂ���(OFF�𐄏����܂����A�������J�N���ꍇ��ON�ɂ��Ă��������B)";
        vsyncCheckBox.Location = new Point(10, 20);
        vsyncCheckBox.AutoSize = true;
        graphicPanel.Controls.Add(vsyncCheckBox);


        Label fpsLabel1 = new Label();
        fpsLabel1.Text = "�t���[�����[�g(��������OFF�̎��̂ݗL��)";
        fpsLabel1.Location = new Point(10, 50);
        fpsLabel1.AutoSize = true;
        graphicPanel.Controls.Add(fpsLabel1);

        Label fpsLabel2 = new Label();
        fpsLabel2.Text = "�l���傫���قǍ����ׂɂȂ�܂����A�L�[��@���Ă��特����܂ł̃��X�|���X���ǂ��Ȃ�܂��B";
        fpsLabel2.Location = new Point(10, 80);
        fpsLabel2.AutoSize = true;
        graphicPanel.Controls.Add(fpsLabel2);

        fpsNumericUpDown.Minimum = 1;
        fpsNumericUpDown.Maximum = 999;
        fpsNumericUpDown.Location = new Point(250, 50);
        fpsNumericUpDown.Size = new Size(70, 30);
        graphicPanel.Controls.Add(fpsNumericUpDown);

        showFpsCheckBox.Text = "�t���[�����[�g��\������";
        showFpsCheckBox.Location = new Point(10, 110);
        showFpsCheckBox.AutoSize = true;
        graphicPanel.Controls.Add(showFpsCheckBox);

        Label soundOutputTypeLabel = new Label();
        soundOutputTypeLabel.Text = "�����o�̓^�C�v";
        soundOutputTypeLabel.Location = new Point(10, 20);
        soundOutputTypeLabel.AutoSize = true;
        soundPanel.Controls.Add(soundOutputTypeLabel);

        soundOutputTypeComboBox.Items.Add("DirectSound");
        soundOutputTypeComboBox.Items.Add("WASAPI");
        soundOutputTypeComboBox.Items.Add("ASIO(�o�b�t�@�T�C�Y�����f����Ȃ����Ƃ����邽�ߔ񐄏�)");
        soundOutputTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        soundOutputTypeComboBox.Location = new Point(100, 20);
        soundOutputTypeComboBox.Size = new Size(300, 30);
        soundOutputTypeComboBox.SelectedIndexChanged += new EventHandler(onTextChanged);
        soundPanel.Controls.Add(soundOutputTypeComboBox);


        wasapiExclusiveCheckBox.Text = "�r�����[�h(ON�ɂ���Ɖ����x�������Ȃ��Ȃ�܂�)";
        wasapiExclusiveCheckBox.Location = new Point(10, 50);
        wasapiExclusiveCheckBox.AutoSize = true;
        soundPanel.Controls.Add(wasapiExclusiveCheckBox);


        asioDriverLabel.Text = "ASIO�h���C�o";
        asioDriverLabel.Location = new Point(10, 50);
        asioDriverLabel.AutoSize = true;
        soundPanel.Controls.Add(asioDriverLabel);

        var drivers = AsioOut.GetDriverNames();

        foreach (var item in drivers)
        {
            asioDriverComboBox.Items.Add(item);
        }
        asioDriverComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        asioDriverComboBox.Location = new Point(100, 50);
        asioDriverComboBox.Size = new Size(300,30);
        soundPanel.Controls.Add(asioDriverComboBox);

        bufferLabel.Text = "�o�b�t�@�T�C�Y(�������������x�������Ȃ��Ȃ�܂����A�m�C�Y�������܂�)";
        bufferLabel.Location = new Point(10, 80);
        bufferLabel.AutoSize = true;
        soundPanel.Controls.Add(bufferLabel);

        bufferNumericUpDown.Minimum = 1;
        bufferNumericUpDown.Maximum = 9999;
        bufferNumericUpDown.Location = new Point(370, 80);
        bufferNumericUpDown.Size = new Size(70, 30);
        soundPanel.Controls.Add(bufferNumericUpDown);

    }

    private void onTextChanged(object? sender, EventArgs e)
    {
        updateSoundUi();
    }
    private void updateSoundUi()
    {
        if (soundOutputTypeComboBox.SelectedIndex == 0)
        {
            soundPanel.Controls.Remove(wasapiExclusiveCheckBox);
            soundPanel.Controls.Remove(asioDriverLabel);
            soundPanel.Controls.Remove(asioDriverComboBox);
            soundPanel.Controls.Remove(bufferLabel);
            soundPanel.Controls.Remove(bufferNumericUpDown);
        }
        else if (soundOutputTypeComboBox.SelectedIndex == 1)
        {
            soundPanel.Controls.Add(wasapiExclusiveCheckBox);
            soundPanel.Controls.Remove(asioDriverLabel);
            soundPanel.Controls.Remove(asioDriverComboBox);
            soundPanel.Controls.Remove(bufferLabel);
            soundPanel.Controls.Remove(bufferNumericUpDown);
        }
        else if (soundOutputTypeComboBox.SelectedIndex == 2)
        {
            soundPanel.Controls.Remove(wasapiExclusiveCheckBox);
            soundPanel.Controls.Add(asioDriverLabel);
            soundPanel.Controls.Add(asioDriverComboBox);
            soundPanel.Controls.Add(bufferLabel);
            soundPanel.Controls.Add(bufferNumericUpDown);

        }
    }


    private void InitTabPage2()
    {
        tabPage2 = new TabPage();
        tabPage2.Name = "tab2";
        tabPage2.Text = "���̑��ݒ�(�ύX�񐄏�)";
        tabControl.TabPages.Add(tabPage2);


        vsyncOffsetCompensationCheckBox.Text = "�����̔���^�C�~���O��1�t���[���x�点��(��������ON�̎��̂ݗL��)";
        vsyncOffsetCompensationCheckBox.Location = new Point(5, 10);
        vsyncOffsetCompensationCheckBox.AutoSize = true;
        tabPage2.Controls.Add(vsyncOffsetCompensationCheckBox);


        showStrShadowCheckBox.Text = "�����̉e��\������(�\�����Ȃ��ꍇ�A�������y���Ȃ�\��������܂�)";
        showStrShadowCheckBox.Location = new Point(5, 40);
        showStrShadowCheckBox.AutoSize = true;
        tabPage2.Controls.Add(showStrShadowCheckBox);


        useHiperformanceTimerCheckBox.Text = "�����x�^�C�}�[���g�p����";
        useHiperformanceTimerCheckBox.Location = new Point(5, 70);
        useHiperformanceTimerCheckBox.AutoSize = true;
        tabPage2.Controls.Add(useHiperformanceTimerCheckBox);


        Label songSelectRowNumberLabel = new Label();
        songSelectRowNumberLabel.Text = "�I�ȉ�ʂŕ\������Ȑ�";
        songSelectRowNumberLabel.Location = new Point(5, 100);
        songSelectRowNumberLabel.AutoSize = true;
        tabPage2.Controls.Add(songSelectRowNumberLabel);

        songSelectRowNumberNumericUpDown.Minimum = 3;
        songSelectRowNumberNumericUpDown.Maximum = 15;
        songSelectRowNumberNumericUpDown.Location = new Point(150, 100);
        songSelectRowNumberNumericUpDown.Size = new Size(70, 30);
        tabPage2.Controls.Add(songSelectRowNumberNumericUpDown);

        Label displayTimingOffsetLabel1 = new Label();
        displayTimingOffsetLabel1.Text = "���ʕ\���^�C�~���O�I�t�Z�b�g(ms)";
        displayTimingOffsetLabel1.Location = new Point(5, 130);
        displayTimingOffsetLabel1.AutoSize = true;
        tabPage2.Controls.Add(displayTimingOffsetLabel1);

        Label displayTimingOffsetLabel2 = new Label();
        displayTimingOffsetLabel2.Text = "�Ⴆ��30�~���b�̒x�ꂪ��������f�B�X�v���C�Ȃ�30�ƕύX���Ă��������B\n���܂�ɂ��傫���l�̏ꍇ�����O�m�[�g�̋��������������Ȃ�܂��B";
        displayTimingOffsetLabel2.Location = new Point(5, 160);
        displayTimingOffsetLabel2.AutoSize = true;
        tabPage2.Controls.Add(displayTimingOffsetLabel2);

        displayTimingOffsetNumericUpDown.Minimum = 0;
        displayTimingOffsetNumericUpDown.Maximum = 1000;
        displayTimingOffsetNumericUpDown.Location = new Point(180, 130);
        displayTimingOffsetNumericUpDown.Size = new Size(70, 30);
        tabPage2.Controls.Add(displayTimingOffsetNumericUpDown);

        fullScreenCheckBox.Text = "�t���X�N���[���ŋN��(����ɓ��삵�Ȃ��\��������܂�)";
        fullScreenCheckBox.Location = new Point(5, 220);
        fullScreenCheckBox.AutoSize = true;
        tabPage2.Controls.Add(fullScreenCheckBox);
    }


    private void InitTabPage3()
    {
        tabPage3 = new TabPage();
        tabPage3.Name = "tab2";
        tabPage3.Text = "�J���җp�ݒ�";
        tabControl.TabPages.Add(tabPage3);
;
        Label note = new Label();
        note.Text = "�J���җp�̐ݒ�ł��B�ύX����K�v�͂���܂���B";
        note.Location = new Point(5, 10);
        note.AutoSize = true;
        tabPage3.Controls.Add(note);

        showDebugCheckBox.Text = "�f�o�b�O���[�h�̕\��";
        showDebugCheckBox.Location = new Point(5,40);
        showDebugCheckBox.AutoSize = true;
        tabPage3.Controls.Add(showDebugCheckBox);

        localCheckBox.Text = "�T�[�o�[�ڑ�������[�J���ɂ���";
        localCheckBox.Location = new Point(5, 70);
        localCheckBox.AutoSize = true;
        tabPage3.Controls.Add(localCheckBox);
        
        usePyCheckBox.Text = "python�X�N���v�g�̎g�p";
        usePyCheckBox.Location = new Point(5, 100);
        usePyCheckBox.AutoSize = true;
        tabPage3.Controls.Add(usePyCheckBox);
    }

    private void InitTabControl()
    {
        tabControl = new TabControl();
        tabControl.Location = new Point(10, 10);
        tabControl.Size = new Size(620, 300);
        this.Controls.Add(tabControl);
    }

    private void InitForm()
    {
        this.Text = "nature prhysm launcher";
        this.MaximizeBox = false;
        this.ClientSize = new Size(640,360);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.Icon = Resources.icon;
    }
}