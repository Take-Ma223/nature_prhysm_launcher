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




    // コンストラクタ
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
        gameStartButton.Text = "ゲームスタート";
        gameStartButton.Click += new EventHandler(onClickGameStart);
        this.Controls.Add(gameStartButton);

        resetButton.Location = new Point(513, 320);
        resetButton.AutoSize = true;
        resetButton.Text = "デフォルト設定に戻す";
        resetButton.Click += new EventHandler(onClickReset);
        this.Controls.Add(resetButton);

        transferSaveDataButton.Location = new Point(310, 320);
        transferSaveDataButton.AutoSize = true;
        transferSaveDataButton.Text = "セーブデータの引継ぎ(ver1.30以降)";
        transferSaveDataButton.Click += new EventHandler(onClickTransferSaveData);
        this.Controls.Add(transferSaveDataButton);
    }

    private void onClickGameStart(object? sender, EventArgs e)
    {
        //設定の保存
        var isSucceed = settingSaveLoad.Save(makeSetting());

        if (isSucceed)
        {
            //ゲームの起動
            System.IO.Directory.SetCurrentDirectory(@"programs/application");

            System.Diagnostics.Process.Start(@"nature_prhysm.exe");
            this.Close();
        }

    }

    private void onClickReset(object? sender, EventArgs e)
    {
        //設定初期化の確認
        DialogResult result = MessageBox.Show(
           "設定値をデフォルトに戻しますか？", "デフォルト設定に戻す",
           MessageBoxButtons.YesNo,    // ボタンの設定
           MessageBoxIcon.Question);   // アイコンの設定

        if (result == DialogResult.Yes)
        {
            var setting = settingSaveLoad.getInitSetting();
            updateSetting(setting);

            MessageBox.Show(
          "設定値をデフォルトに戻しました。", "デフォルト設定に戻す",
          MessageBoxButtons.OK,    // ボタンの設定
          MessageBoxIcon.Information);   // アイコンの設定
        }

        //設定の初期化
    }

    private void onClickTransferSaveData(object? sender, EventArgs e)
    {
        SaveDataTransfer saveDataTransfer = new SaveDataTransfer();


        var result = MessageBox.Show(
           "セーブデータを引き継ぎたいnature prhysmのフォルダを選択してください(ver1.30以降)。\n移動先のデータは削除されます。", "セーブデータ引継ぎ",
           MessageBoxButtons.OKCancel,    // ボタンの設定
           MessageBoxIcon.Information);   // アイコンの設定

        if (result == DialogResult.Cancel) return;

        FolderBrowserDialog dialog = new FolderBrowserDialog()
        {
            SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),  // 選択されるフォルダの初期値
            RootFolder = Environment.SpecialFolder.Desktop,  // ルート
            Description = "セーブデータを引き継ぎたいnature prhysmのフォルダを選択してください。",   // 説明文
        };

        result = dialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            if(System.Environment.CurrentDirectory == dialog.SelectedPath)//自身と同じフォルダは選ばせない
            {
                MessageBox.Show("引継ぎ先と同じフォルダは選べません。他のフォルダを選択してください。", "セーブデータ引継ぎ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var isSucceed = saveDataTransfer.transfer(dialog.SelectedPath);
            if (isSucceed) loadSetting();
        }

        //はいを選んだらセーブデータの引継ぎ処理
    }


    private void InitTabPage1()
    {
        tabPage1 = new TabPage();
        tabPage1.Name = "tab1";
        tabPage1.Text = "基本設定";
        tabControl.TabPages.Add(tabPage1);

        graphicPanel.Text = "グラフィック";
        graphicPanel.Location = new Point(10, 0);
        graphicPanel.Size = new Size(590,145);
        tabPage1.Controls.Add(graphicPanel);

        soundPanel.Text = "サウンド";
//        soundPanel.BorderStyle = BorderStyle.FixedSingle;
        soundPanel.Location = new Point(10, 150);
        soundPanel.Size = new Size(590, 110);
        tabPage1.Controls.Add(soundPanel);

        vsyncCheckBox.Text = "垂直同期をONにする(OFFを推奨しますが、音符がカクつく場合はONにしてください。)";
        vsyncCheckBox.Location = new Point(10, 20);
        vsyncCheckBox.AutoSize = true;
        graphicPanel.Controls.Add(vsyncCheckBox);


        Label fpsLabel1 = new Label();
        fpsLabel1.Text = "フレームレート(垂直同期OFFの時のみ有効)";
        fpsLabel1.Location = new Point(10, 50);
        fpsLabel1.AutoSize = true;
        graphicPanel.Controls.Add(fpsLabel1);

        Label fpsLabel2 = new Label();
        fpsLabel2.Text = "値が大きいほど高負荷になりますが、キーを叩いてから音が鳴るまでのレスポンスが良くなります。";
        fpsLabel2.Location = new Point(10, 80);
        fpsLabel2.AutoSize = true;
        graphicPanel.Controls.Add(fpsLabel2);

        fpsNumericUpDown.Minimum = 1;
        fpsNumericUpDown.Maximum = 999;
        fpsNumericUpDown.Location = new Point(250, 50);
        fpsNumericUpDown.Size = new Size(70, 30);
        graphicPanel.Controls.Add(fpsNumericUpDown);

        showFpsCheckBox.Text = "フレームレートを表示する";
        showFpsCheckBox.Location = new Point(10, 110);
        showFpsCheckBox.AutoSize = true;
        graphicPanel.Controls.Add(showFpsCheckBox);

        Label soundOutputTypeLabel = new Label();
        soundOutputTypeLabel.Text = "音声出力タイプ";
        soundOutputTypeLabel.Location = new Point(10, 20);
        soundOutputTypeLabel.AutoSize = true;
        soundPanel.Controls.Add(soundOutputTypeLabel);

        soundOutputTypeComboBox.Items.Add("DirectSound");
        soundOutputTypeComboBox.Items.Add("WASAPI");
        soundOutputTypeComboBox.Items.Add("ASIO(バッファサイズが反映されないことがあるため非推奨)");
        soundOutputTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        soundOutputTypeComboBox.Location = new Point(100, 20);
        soundOutputTypeComboBox.Size = new Size(300, 30);
        soundOutputTypeComboBox.SelectedIndexChanged += new EventHandler(onTextChanged);
        soundPanel.Controls.Add(soundOutputTypeComboBox);


        wasapiExclusiveCheckBox.Text = "排他モード(ONにすると音声遅延が少なくなります)";
        wasapiExclusiveCheckBox.Location = new Point(10, 50);
        wasapiExclusiveCheckBox.AutoSize = true;
        soundPanel.Controls.Add(wasapiExclusiveCheckBox);


        asioDriverLabel.Text = "ASIOドライバ";
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

        bufferLabel.Text = "バッファサイズ(小さい程音声遅延が少なくなりますが、ノイズが増えます)";
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
        tabPage2.Text = "その他設定(変更非推奨)";
        tabControl.TabPages.Add(tabPage2);


        vsyncOffsetCompensationCheckBox.Text = "音符の判定タイミングを1フレーム遅らせる(垂直同期ONの時のみ有効)";
        vsyncOffsetCompensationCheckBox.Location = new Point(5, 10);
        vsyncOffsetCompensationCheckBox.AutoSize = true;
        tabPage2.Controls.Add(vsyncOffsetCompensationCheckBox);


        showStrShadowCheckBox.Text = "文字の影を表示する(表示しない場合、処理が軽くなる可能性があります)";
        showStrShadowCheckBox.Location = new Point(5, 40);
        showStrShadowCheckBox.AutoSize = true;
        tabPage2.Controls.Add(showStrShadowCheckBox);


        useHiperformanceTimerCheckBox.Text = "高精度タイマーを使用する";
        useHiperformanceTimerCheckBox.Location = new Point(5, 70);
        useHiperformanceTimerCheckBox.AutoSize = true;
        tabPage2.Controls.Add(useHiperformanceTimerCheckBox);


        Label songSelectRowNumberLabel = new Label();
        songSelectRowNumberLabel.Text = "選曲画面で表示する曲数";
        songSelectRowNumberLabel.Location = new Point(5, 100);
        songSelectRowNumberLabel.AutoSize = true;
        tabPage2.Controls.Add(songSelectRowNumberLabel);

        songSelectRowNumberNumericUpDown.Minimum = 3;
        songSelectRowNumberNumericUpDown.Maximum = 15;
        songSelectRowNumberNumericUpDown.Location = new Point(150, 100);
        songSelectRowNumberNumericUpDown.Size = new Size(70, 30);
        tabPage2.Controls.Add(songSelectRowNumberNumericUpDown);

        Label displayTimingOffsetLabel1 = new Label();
        displayTimingOffsetLabel1.Text = "譜面表示タイミングオフセット(ms)";
        displayTimingOffsetLabel1.Location = new Point(5, 130);
        displayTimingOffsetLabel1.AutoSize = true;
        tabPage2.Controls.Add(displayTimingOffsetLabel1);

        Label displayTimingOffsetLabel2 = new Label();
        displayTimingOffsetLabel2.Text = "例えば30ミリ秒の遅れが発生するディスプレイなら30と変更してください。\nあまりにも大きい値の場合ロングノートの挙動がおかしくなります。";
        displayTimingOffsetLabel2.Location = new Point(5, 160);
        displayTimingOffsetLabel2.AutoSize = true;
        tabPage2.Controls.Add(displayTimingOffsetLabel2);

        displayTimingOffsetNumericUpDown.Minimum = 0;
        displayTimingOffsetNumericUpDown.Maximum = 1000;
        displayTimingOffsetNumericUpDown.Location = new Point(180, 130);
        displayTimingOffsetNumericUpDown.Size = new Size(70, 30);
        tabPage2.Controls.Add(displayTimingOffsetNumericUpDown);

        fullScreenCheckBox.Text = "フルスクリーンで起動(正常に動作しない可能性があります)";
        fullScreenCheckBox.Location = new Point(5, 220);
        fullScreenCheckBox.AutoSize = true;
        tabPage2.Controls.Add(fullScreenCheckBox);
    }


    private void InitTabPage3()
    {
        tabPage3 = new TabPage();
        tabPage3.Name = "tab2";
        tabPage3.Text = "開発者用設定";
        tabControl.TabPages.Add(tabPage3);
;
        Label note = new Label();
        note.Text = "開発者用の設定です。変更する必要はありません。";
        note.Location = new Point(5, 10);
        note.AutoSize = true;
        tabPage3.Controls.Add(note);

        showDebugCheckBox.Text = "デバッグモードの表示";
        showDebugCheckBox.Location = new Point(5,40);
        showDebugCheckBox.AutoSize = true;
        tabPage3.Controls.Add(showDebugCheckBox);

        localCheckBox.Text = "サーバー接続先をローカルにする";
        localCheckBox.Location = new Point(5, 70);
        localCheckBox.AutoSize = true;
        tabPage3.Controls.Add(localCheckBox);
        
        usePyCheckBox.Text = "pythonスクリプトの使用";
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