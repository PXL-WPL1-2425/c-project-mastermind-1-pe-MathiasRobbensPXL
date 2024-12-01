using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //timer voor tijd bij te houden
        private DispatcherTimer countdownTimer;
        private int countdownTime = 0;

        //lijst aanmaken voor de verschillende kleuren
        List<string> colors = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        List<string> chosenColors = new List<string>();

        //aanmaken van variabele van attempts
        private int attempts = 1;

        // Variabele voor de kleurcode die we willen tonen in de titel
        private string colorCodeString = "";
        public MainWindow()
        {
            InitializeComponent();
            

            //hier timer initialiseren
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);//elke seconden ticken
            countdownTimer.Tick += CountdownTimer_Tick;

            ComboBoxes();

            RandomColors();

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }
        // Methode om de countdown timer te starten (of opnieuw te starten)
        private void startCountdown()
        {
            countdownTime = 1;  // Zet de tijd op 1 seconde bij elke start
            countdownTimer.Start(); // Start de timer
            timerLabel.Content = $"Tijd: {countdownTime} sec";  // Toon de timer op de UI
        }

        // Event handler voor de countdown timer
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownTime++;  // Verhoog de tijd elke seconde
            timerLabel.Content = $"Tijd: {countdownTime} sec";  // Werk de timer bij in de UI
        }


        // Event handler voor de toetscombinatie CTRL+F12
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Controleer of CTRL+F12 is ingedrukt
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.Key == Key.F12)
            {
                ToggleDebug(); 
            }
        }
       
        
        // Methode om debug-modus in te schakelen of uit te schakelen
        private void ToggleDebug()
        {
            // Als de debugTextBox zichtbaar is, verberg het en anders toon je de textbox
            if (debugTextBox.Visibility == Visibility.Collapsed)
            {
                debugTextBox.Visibility = Visibility.Visible;
                debugTextBox.Text = colorCodeString;
            }
            else
            {
                debugTextBox.Visibility = Visibility.Collapsed;
            }
        }
        private void RandomColors()
        {
            
            // 4 kleuren leegmaken anders kan dit problemen veroorzaken
            chosenColors.Clear();

            // genereer random kleuren, 4 stuks
            Random random = new Random();
            for (int i = 0; i < 4; i++) 
            { 
             int index = random.Next(colors.Count);
                chosenColors.Add(colors[index]);
            
            }
            string colorstring = string.Join(",", chosenColors);

            //titel updaten met de code
            colorCodeString = string.Join(",", chosenColors);

            Title = $"Mastermind - Poging {attempts}";

            startCountdown();
        }

        private void ComboBoxes()
        {
            comboBox1.ItemsSource = colors;
            comboBox2.ItemsSource = colors;
            comboBox3.ItemsSource = colors;
            comboBox4.ItemsSource = colors;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Als er een kleur is geselecteerd in combobox 1, toon deze in label 1
            //if (comboBox1.SelectedItem != null)
            //label1.Content = comboBox1.SelectedItem.ToString();

            // Doe hetzelfde voor de andere comboboxen en labels
            //if (comboBox2.SelectedItem != null)
            //label2.Content = comboBox2.SelectedItem.ToString();
            //if (comboBox3.SelectedItem != null)
            //label3.Content = comboBox3.SelectedItem.ToString();
            //if (comboBox4.SelectedItem != null)
            //label4.Content = comboBox4.SelectedItem.ToString();

            //Deze code gaf enkel de naam weer van de kleur in het bijhorende label, moet worden bijgewerkt.

            if (comboBox1.SelectedItem != null)
                label1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox1.SelectedItem.ToString()));
            if (comboBox2.SelectedItem != null)
                label2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox2.SelectedItem.ToString()));
            if (comboBox3.SelectedItem != null)
                label3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox3.SelectedItem.ToString()));
            if (comboBox4.SelectedItem != null)
                label4.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox4.SelectedItem.ToString()));

        }

        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            // Verkrijg de geselecteerde kleuren van de comboboxen
            string selectedColor1 = comboBox1.SelectedItem?.ToString();
            string selectedColor2 = comboBox2.SelectedItem?.ToString();
            string selectedColor3 = comboBox3.SelectedItem?.ToString();
            string selectedColor4 = comboBox4.SelectedItem?.ToString();

            // check die controleert of elke combobox iets heeft ingevuld
            if (selectedColor1 == null || selectedColor2 == null || selectedColor3 == null || selectedColor4 == null)
            {
                MessageBox.Show("Kies alstublieft een kleur voor elke combobox.");
                return;
            }

            // Zet de borders van de labels terug naar de standaard (geen rand)
            label1.BorderBrush = null;
            label2.BorderBrush = null;
            label3.BorderBrush = null;
            label4.BorderBrush = null;

            // Controleer voor elke kleur of deze juist is (kleur en positie)
            if (selectedColor1 == chosenColors[0]) label1.BorderBrush = new SolidColorBrush(Colors.DarkRed); // Correcte kleur op de juiste plaats
            else if (chosenColors.Contains(selectedColor1)) label1.BorderBrush = new SolidColorBrush(Colors.Wheat); // Kleur aanwezig, maar op de verkeerde plaats

            if (selectedColor2 == chosenColors[1]) label2.BorderBrush = new SolidColorBrush(Colors.DarkRed); 
            else if (chosenColors.Contains(selectedColor2)) label2.BorderBrush = new SolidColorBrush(Colors.Wheat);

            if (selectedColor3 == chosenColors[2]) label3.BorderBrush = new SolidColorBrush(Colors.DarkRed); 
            else if (chosenColors.Contains(selectedColor3)) label3.BorderBrush = new SolidColorBrush(Colors.Wheat); 

            if (selectedColor4 == chosenColors[3]) label4.BorderBrush = new SolidColorBrush(Colors.DarkRed); 
            else if (chosenColors.Contains(selectedColor4)) label4.BorderBrush = new SolidColorBrush(Colors.Wheat); 

            //Hier updaten we de attempts in de 
            attempts++;
            Title = $"Mastermind - Poging {attempts}";

            startCountdown();
        }
    }
}