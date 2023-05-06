using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nature_prhysm_launcher
{
    internal class SaveDataTransfer
    {
        /// <summary>
        /// データの引継ぎ
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true:成功 false:失敗</returns>
        public bool transfer(string path)
        {
            var save_data_dir = Path.Combine(path, @"save_data");
            if (Directory.Exists(save_data_dir))
            {
                try
                {
                    //コピー先削除
                    string dest_path = @"save_data";
                    Directory.Delete(dest_path, true);

                    // コピー実行
                    CopyDirectory(save_data_dir, dest_path, true);

                    MessageBox.Show("セーブデータの引継ぎが完了しました。", "セーブデータ引継ぎ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch(System.IO.DirectoryNotFoundException e)
                {
                    MessageBox.Show("引継ぎ先にsave_dataフォルダが見つからないため、\nセーブデータを引き継げません。\nsave_dataフォルダを作成してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                catch (System.IO.IOException e)
                {
                    MessageBox.Show("他のアプリケーションによって開かれているファイルがあるため、\nセーブデータを引き継げません。\nファイルを開いているアプリケーションを終了してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            else
            {
                MessageBox.Show("save_dataフォルダが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }













    }
}
