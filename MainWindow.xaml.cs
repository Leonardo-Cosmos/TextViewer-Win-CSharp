/* 2021/4/14 */
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextViewer.Settings;

namespace TextViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextViewerSetting setting;

        private ObservableCollection<string> jsonPaths = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();

            comboBoxJsonPath.ItemsSource = jsonPaths;
        }

        private void TextBoxPlain_TextChanged(object sender, TextChangedEventArgs e)
        {
            ParsePlainText();
        }

        private void ComboBoxJsonPath_LostFocus(object sender, RoutedEventArgs e)
        {
            ParsePlainText();
        }

        private void TextBoxPlain_LostFocus(object sender, RoutedEventArgs e)
        {
            ParsePlainText();
        }

        private void ParsePlainText()
        {
            var plaintext = textBoxPlain.Text;
            if (String.IsNullOrEmpty(plaintext))
            {
                return;
            }

            var jsonPath = comboBoxJsonPath.Text;
            if (String.IsNullOrEmpty(jsonPath))
            {
                jsonPath = comboBoxJsonPath.SelectedItem as String;
            }

            if (String.IsNullOrEmpty(jsonPath))
            {
                comboBoxJsonPath.BorderBrush = Brushes.Red;
                return;
            }
            else
            {
                comboBoxJsonPath.BorderBrush = Brushes.Transparent;
            }
            var jsonPathPropertyNames = jsonPath.Split(".");

            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(plaintext);
                textBoxPlain.BorderBrush = Brushes.Transparent;

                object currentElement = dict;
                foreach (var jsonPathPropName in jsonPathPropertyNames)
                {
                    if (currentElement is Dictionary<string, object>)
                    {
                        currentElement = (currentElement as Dictionary<string, object>)[jsonPathPropName];
                    }
                }

                textBlockDocument.Text = currentElement.ToString();
            } 
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                textBoxPlain.BorderBrush = Brushes.Red;
            }

            jsonPaths.Insert(0, jsonPath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setting = SettingSerializer.Load() ?? new TextViewerSetting();

            var jsonPathsSetting = setting?.JsonText?.JsonPaths ?? new List<string>(0);
            jsonPathsSetting.ForEach(jsonPath => jsonPaths.Add(jsonPath));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var jsonTextSetting = setting.JsonText ??= new JsonTextSetting();
            var jsonPathsSetting = jsonTextSetting.JsonPaths ??= new List<string>();
            jsonPathsSetting.AddRange(jsonPaths);

            SettingSerializer.Save(setting);
        }

        private void ButtonDeleteJsonPath_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
