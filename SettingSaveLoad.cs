﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace nature_prhysm_launcher
{
    internal class SettingSaveLoad
    {
        Setting setting;

        // 書き込むファイルのパス
        string path = @"save_data/config.dat";

        Dictionary<string, string> settingLabel = new Dictionary<string, string>()
        {
            { "VSYNC",                      "VSYNC" },
            { "FPS",                        "FPS" },
            { "SHOW_FPS",                   "SHOW_FPS" },
            { "SOUND_OUTPUT_TYPE",          "SOUND_OUTPUT_TYPE" },
            { "WASAPI_EXCLUSIVE",           "WASAPI_EXCLUSIVE" },
            { "ASIO_DRIVER",                "ASIO_DRIVER" },
            { "BUFFER",                     "BUFFER" },

            { "NOTE_TEXT_R","NOTE_TEXT_R"},
            { "NOTE_TEXT_G","NOTE_TEXT_G"},
            { "NOTE_TEXT_B","NOTE_TEXT_B"},
            { "NOTE_TEXT_C","NOTE_TEXT_C"},
            { "NOTE_TEXT_M","NOTE_TEXT_M"},
            { "NOTE_TEXT_Y","NOTE_TEXT_Y"},
            { "NOTE_TEXT_W","NOTE_TEXT_W"},
            { "NOTE_TEXT_K","NOTE_TEXT_K"},
            { "NOTE_TEXT_F","NOTE_TEXT_F"},

            { "VSYNC_OFFSET_COMPENSATION",  "VSYNC_OFFSET_COMPENSATION" },
            { "SHOW_STR_SHADOW",            "SHOW_STR_SHADOW" },
            { "USE_HIPERFORMANCE_TIMER",    "USE_HIPERFORMANCE_TIMER" },
            { "SONG_SELECT_ROW_NUMBER",     "SONG_SELECT_ROW_NUMBER" },
            { "DISPLAY_TIMING_OFFSET",      "DISPLAY_TIMING_OFFSET" },
            { "FULLSCREEN",                 "FULLSCREEN" },
            { "USE_AI_PREDICTED_DIFFICULTY","USE_AI_PREDICTED_DIFFICULTY" },

            { "SHOW_DEBUG",                 "SHOW_DEBUG" },
            { "LOCAL",                      "LOCAL" },
            { "USE_PY",                     "USE_PY" },
            { "COM_PORT",                   "COM_PORT" },
            { "EDITABLE",                   "EDITABLE" },
        };


        /// <summary>
        /// ファイルに書き込み
        /// </summary>
        /// <param name="setting">設定値</param>
        /// <returns>true:成功 false:失敗</returns>
        public bool Save(Setting setting) {
            //書き込む内容
            List<string> text = new List<string>
            {
                string.Format("{0}:{1}",settingLabel["VSYNC"],getWriteText(setting.vsync)),
                string.Format("{0}:{1}",settingLabel["FPS"],setting.fps),
                string.Format("{0}:{1}",settingLabel["SHOW_FPS"],getWriteText(setting.showFps)),
                string.Format("{0}:{1}",settingLabel["SOUND_OUTPUT_TYPE"],setting.soundOutputType),
                string.Format("{0}:{1}",settingLabel["WASAPI_EXCLUSIVE"],getWriteText(setting.wasapiExclusive)),
                string.Format("{0}:{1}",settingLabel["ASIO_DRIVER"],setting.asioDriver),
                string.Format("{0}:{1}",settingLabel["BUFFER"],setting.buffer),
                string.Format("{0}:{1}",settingLabel["VSYNC_OFFSET_COMPENSATION"],getWriteText(setting.vsyncOffsetCompensation)),

                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_R"],getWriteText(setting.noteText[0])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_G"],getWriteText(setting.noteText[1])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_B"],getWriteText(setting.noteText[2])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_C"],getWriteText(setting.noteText[3])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_M"],getWriteText(setting.noteText[4])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_Y"],getWriteText(setting.noteText[5])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_W"],getWriteText(setting.noteText[6])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_K"],getWriteText(setting.noteText[7])),
                string.Format("{0}:{1}",settingLabel["NOTE_TEXT_F"],getWriteText(setting.noteText[8])),

                string.Format("{0}:{1}",settingLabel["SHOW_STR_SHADOW"],getWriteText(setting.showStrShadow)),
                string.Format("{0}:{1}",settingLabel["USE_HIPERFORMANCE_TIMER"],getWriteText(setting.useHiperformanceTimer)),
                string.Format("{0}:{1}",settingLabel["SONG_SELECT_ROW_NUMBER"],setting.songSelectRowNumber),                        
                string.Format("{0}:{1}",settingLabel["DISPLAY_TIMING_OFFSET"],setting.displayTimingOffset),                   
                string.Format("{0}:{1}",settingLabel["FULLSCREEN"],getWriteText(setting.fullScreen)),
                string.Format("{0}:{1}",settingLabel["EDITABLE"],getWriteText(setting.editable)),
                string.Format("{0}:{1}",settingLabel["USE_AI_PREDICTED_DIFFICULTY"],getWriteText(setting.useAiPredictedDifficulty)),

                string.Format("{0}:{1}",settingLabel["SHOW_DEBUG"],getWriteText(setting.showDebug)),
                string.Format("{0}:{1}",settingLabel["LOCAL"],getWriteText(setting.local)),
                string.Format("{0}:{1}",settingLabel["USE_PY"],getWriteText(setting.usePy)),
                string.Format("{0}:{1}",settingLabel["COM_PORT"],setting.comPort),

            };
            try
            {
                // ファイルに書き込む
                File.WriteAllLines(path, text, Encoding.GetEncoding("Unicode"));
                return true;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show("save_dataフォルダが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("他のアプリケーションによってconfig.datファイルが開かれているため、\n設定を書き込めません。\nファイルを開いているアプリケーションを終了してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public Setting Load() {
            setting = new Setting();

            string[] lines;

            try
            {
                // ファイルに書き込む
                lines = File.ReadAllLines(path, Encoding.GetEncoding("Unicode"));
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show("save_dataフォルダが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //見つからなかったらデフォルト値
                return getInitSetting();
            }
            catch (System.IO.IOException)
            {
                //見つからなかったらデフォルト値
                return getInitSetting();
            }

            
            // 1行ずつ読み込んで表示
            foreach (var line in lines)
            {
                var split = line.Split(':');
                if (split.Length == 2)
                { 
                    try
                    {
                        var value = int.Parse(split[1]);
                        parseSetting(split[0], value);
                    }
                    catch (System.FormatException ex)
                    {
                        MessageBox.Show("Config.datを読み込めませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return getInitSetting();
                    }
                }
            }

            return setting;
        }

        private void parseSetting(string v, int value)
        {               
            if (v == settingLabel["VSYNC"]) setting.vsync = intToBool(value);
            if (v == settingLabel["FPS"]) setting.fps = value;
            if (v == settingLabel["SHOW_FPS"]) setting.showFps = intToBool(value);
            if (v == settingLabel["SOUND_OUTPUT_TYPE"]) setting.soundOutputType = value;
            if (v == settingLabel["WASAPI_EXCLUSIVE"]) setting.wasapiExclusive = intToBool(value);
            if (v == settingLabel["ASIO_DRIVER"]) setting.asioDriver = value;
            if (v == settingLabel["BUFFER"]) setting.buffer = value;

            if (v == settingLabel["NOTE_TEXT_R"]) setting.noteText[0] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_G"]) setting.noteText[1] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_B"]) setting.noteText[2] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_C"]) setting.noteText[3] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_M"]) setting.noteText[4] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_Y"]) setting.noteText[5] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_W"]) setting.noteText[6] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_K"]) setting.noteText[7] = intToBool(value);
            if (v == settingLabel["NOTE_TEXT_F"]) setting.noteText[8] = intToBool(value);

            if (v == settingLabel["VSYNC_OFFSET_COMPENSATION"]) setting.vsyncOffsetCompensation = intToBool(value);
            if (v == settingLabel["SHOW_STR_SHADOW"]) setting.showStrShadow = intToBool(value);
            if (v == settingLabel["USE_HIPERFORMANCE_TIMER"]) setting.useHiperformanceTimer = intToBool(value);
            if (v == settingLabel["SONG_SELECT_ROW_NUMBER"]) setting.songSelectRowNumber = value;
            if (v == settingLabel["DISPLAY_TIMING_OFFSET"]) setting.displayTimingOffset = value;
            if (v == settingLabel["FULLSCREEN"]) setting.fullScreen = intToBool(value);
            if (v == settingLabel["EDITABLE"]) setting.editable = intToBool(value);
            if (v == settingLabel["USE_AI_PREDICTED_DIFFICULTY"]) setting.useAiPredictedDifficulty = intToBool(value);

            if (v == settingLabel["SHOW_DEBUG"]) setting.showDebug = intToBool(value);
            if (v == settingLabel["LOCAL"]) setting.local = intToBool(value);
            if (v == settingLabel["USE_PY"]) setting.usePy = intToBool(value);
            if (v == settingLabel["COM_PORT"]) setting.comPort = value;

        }

        private bool intToBool(int value)
        {
            if (value == 0) return false;
            else return true;
        }


        public Setting getInitSetting()
        {
            return new Setting();
        }

        /// <summary>
        /// 書き込み用テキスト(0,1)への変換
        /// </summary>
        /// <param name="b">ブール値</param>
        /// <returns></returns>
        private string getWriteText(bool b)
        {
            if (b) return "1";
            else return "0";
        }

    }

    public struct Setting
    {
        public bool vsync = false;
        public int fps = 450;
        public bool showFps = false;
        public int soundOutputType = 1;
        public bool wasapiExclusive = true;
        public int asioDriver = -1;
        public int buffer = 256;

        public bool[] noteText = { false, false, false, false, false, false, false, false, false };

        public bool vsyncOffsetCompensation = false;
        public bool showStrShadow = true;
        public bool useHiperformanceTimer = true;
        public int songSelectRowNumber = 15;
        public int displayTimingOffset = 0;
        public bool fullScreen = false;
        public bool editable = false;
        public bool useAiPredictedDifficulty = false;

        public bool showDebug = true;
        public bool local = false;
        public bool usePy = false;
        public int comPort = 0;


        public Setting()
        {
        }
    }
}
