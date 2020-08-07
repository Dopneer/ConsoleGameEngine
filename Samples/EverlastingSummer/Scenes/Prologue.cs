using System;
using System.Threading;

namespace ConsoleGameEngine.Scenes
{
    public class Prologue : IScene
    {

        public static void Story(Core core, DialogField dialogField)
        {

            core.DrawContent();

            Thread Animator;

            ConsoleKey Input;

            GameObject Mush = new GameObject(0, 0, new Symbol[Program.SizeY, Program.SizeX], false);

            Mush.Update.Add(new GameObjectDelegate(Mush =>
            {
                byte[] BadColors = new byte[] { 0xFC, 0xFD, 0xFE, 0xFF };

                for (int i = 0; i < Mush.Content.GetLength(0); i++)
                {
                    for (int j = 0; j < Mush.Content.GetLength(1); j++)
                    {
                        Mush.Content[i, j] = new Symbol(' ', 0x52);
                    }
                }

                // Random count of bad lines
                for (int i = 0; i < new Random().Next((int)Math.Ceiling((float)Program.SizeY / 100 * 10), (int)Math.Ceiling((float)Program.SizeY / 100 * 20)); i++)
                {
                    int Height = new Random().Next(0, Mush.Content.GetLength(0)); // Random height to bad line

                    // For all width
                    for (int j = 0; j < Mush.Content.GetLength(1); j++)
                    {
                        Mush.Content[Height, j] = new Symbol(' ', BadColors[new Random().Next(0, BadColors.Length)]);
                    }
                }
            }));


            core.Effect = Mush;

            // Example of animation

            // Update thread
            /* Animator = new Thread(new ThreadStart(dialogField.Animation));
            Animator.Start();

            // While thread active
            while(Animator.IsAlive)
            {
                // Update frame
                core.DrawContent();

                Thread.Sleep(50);
            } */

            dialogField.DrawText("Мне опять снился сон.");

            dialogField.ClearField();

            dialogField.DrawText("Этот сон...");

            dialogField.ClearField();

            dialogField.DrawText("Каждую ночь одно и тоже.");

            dialogField.ClearField();

            dialogField.DrawText("Но наутро, как обычно, все забудется.");

            dialogField.ClearField();

            dialogField.DrawText("Может быть, оно и к лучшему...");

            dialogField.ClearField();

            dialogField.DrawText("Останутся только туманные воспоминания о приоткрытых, словно приглашающих куда-то воротах, рядом с которыми в камне застыли два пионера.");

            dialogField.ClearField();

            dialogField.DrawText("А еще странная девочка...");

            dialogField.DrawText(" которая постоянно спрашивает:");

            dialogField.ClearField();

            // Переход


            dialogField.DrawText("Ты пойдешь со мной?");

            dialogField.ClearField();

            dialogField.DrawText("Пойду?..");

            dialogField.ClearField();

            dialogField.DrawText("Но куда?");

            dialogField.ClearField();

            dialogField.DrawText("И зачем?..");

            dialogField.ClearField();

            dialogField.DrawText("Да и где я вообще нахожусь?");

            dialogField.ClearField();

            dialogField.DrawText("Конечно, случись все на самом деле, наяву, стоило бы непременно испугаться.");

            dialogField.ClearField();

            dialogField.DrawText("Как же иначе!");

            dialogField.ClearField();

            dialogField.DrawText("Но это - всего лишь сон.");

            dialogField.DrawText(" Тот самый, который я вижу каждую ночь.");

            dialogField.ClearField();

            dialogField.DrawText("А ведь все это неспроста!");

            dialogField.ClearField();

            dialogField.DrawText("Необязательно знать где и почему, чтобы понять - что-то происходит.");

            dialogField.ClearField();

            dialogField.DrawText("Нечто, отчаянно требующее моего внимания.");

            dialogField.ClearField();

            dialogField.DrawText("Ведь все окружающее меня здесь - реально!");

            dialogField.ClearField();

            dialogField.DrawText("Реально настолько, насколько реальны вещи в моей квартире; я бы мог открыть ворота, услышать скрип петель, смахнуть рукой сыпающуюся ржавчину, потянуть носом свежий прохладный воздух и поежится от холода.");

            dialogField.ClearField();

            dialogField.DrawText("Мог бы, но для этого надо сдвинуться с места, сделать шаг, пошевелить рукой...");

            dialogField.ClearField();

            dialogField.DrawText("А ведь это сон - я понимаю, но что дальше, что изменит мое понимание?");

            dialogField.ClearField();

            dialogField.DrawText("Ведь здесь - словну по ту сторону потрескавшегося экрана старого телевизора, который из последних сил борется с помехами и силится показать зрителям все, не упустив ни малейшей детали.");

            dialogField.ClearField();

            dialogField.DrawText("Но вот картинка теряет четкость...");

            dialogField.DrawText(" Наверное, скоро просыпаться.");

            dialogField.ClearField();

            dialogField.DrawText("...", 60);

            dialogField.ClearField();

            dialogField.DrawText("Может быть, спросить у нее что-то?");

            dialogField.DrawText(" У девочки.");

            dialogField.ClearField();

            dialogField.DrawText("Как же ее зовут...");

            dialogField.ClearField();

            dialogField.DrawText("Например про звезды...");

            dialogField.ClearField();

            dialogField.DrawText("Хотя почему про звезды?");

            dialogField.ClearField();

            dialogField.DrawText("Можно же спросить про ворота!");

            dialogField.DrawText(" Да, про ворота!");

            dialogField.ClearField();

            dialogField.DrawText("Вот она удивится.");

            dialogField.ClearField();

            dialogField.DrawText("Или лучше про букву ё"); // Блять как же я ненавижу писать пукву е (ну вы поняли), всегда е пишу, а тут надо эту букву...

            dialogField.ClearField();

            dialogField.DrawText("Хорошая была буква...");

            dialogField.ClearField();

            dialogField.DrawText("Как будто ее больше нет!");

            dialogField.ClearField();

            dialogField.DrawText("И какое отношение буквы, ворота и звезды имеют к этому месту?");

            dialogField.ClearField();

            dialogField.DrawText("Ведь если мне каждую ночь снится этот сон, который потом все равно забудется, надо искать разгадку здесь и сейчас!");

            dialogField.ClearField();

            dialogField.DrawText("А вот, если присмотреться, можно увидеть Магелланово облако...");

            dialogField.ClearField();

            dialogField.DrawText("Словно попал в южное полушарие!");

            dialogField.ClearField();

            dialogField.DrawText("...");

            dialogField.ClearField();

            dialogField.DrawText("Во сне всегда больше волнуют мелочи: неестественный цвет травы, невозможная кривизна прямых или свое перекошенное отражение - а реальная опасность, готовая оборвать все здесь и сейчас, кажется пустяком.");

            dialogField.ClearField();

            dialogField.DrawText("Естественно, ведь здесь нельзя умереть.");

            dialogField.ClearField();

            dialogField.DrawText("Я точно знаю - я делал это сотни раз.");

            dialogField.ClearField();

            dialogField.DrawText("Но если нельзя умереть, нет смысла жить?");

            dialogField.ClearField();

            dialogField.DrawText("Надо будет спросить у девочки: она местная - должна знать!");

            dialogField.ClearField();

            dialogField.DrawText("Да, именно!");

            dialogField.DrawText(" Спросить, например, про сову.");

            dialogField.ClearField();

            dialogField.DrawText("Больно уж птица странная...");

            dialogField.ClearField();

            dialogField.DrawText("А врочем, не важно...");

            dialogField.ClearField();

            dialogField.DrawText("...");

            dialogField.ClearField();

            dialogField.DrawText("Ты пойдешь со мной?");

            dialogField.ClearField();

            dialogField.DrawText("И каждый раз надо отвечать.");

            dialogField.ClearField();

            dialogField.DrawText("Иначе никак, иначе сон не закончится, а я - не проснусь.");

            dialogField.ClearField();



            // Это как в ХТМЛ ставим объект по середине
            Select select = new Select(10, (Program.SizeX / 2) - (70 / 2), new Symbol[5, 70], new string[] { "1. Да, я пойду с тобой", "2. Нет, я останусь здесь" });

            Input = select.UserSelect(new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2 });

            Program.Inputs.Add(Input);

            if(Input == ConsoleKey.D1)
            {
                Program.GoWithMe = true;
            }
            else
            {
                Program.GoWithMe = false;
            }




        }

    }
}
