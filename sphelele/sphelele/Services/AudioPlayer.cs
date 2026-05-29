using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace sphelele.Services;

public class AudioPlayer
{
    void Greeting(){
        try{
         SoundPlayer player = new SoundPlayer("Assets/Greeting.wav");
         player.Play();
        }catch(Exception ex){
           MessageBox.Show($"Erro Paying the sound:{ex.Message}");
            Console.WriteLine($"Erro Paying the sound:{ex.Message}");

        }
    }
}