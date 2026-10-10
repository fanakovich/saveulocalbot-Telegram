
using System.Text;
using System.Text.RegularExpressions;
using TL;

public class Program
{
    public static async Task Main(string[] args) // чуть баловался с ии в первой версии была проблемма по какой-то причине не находило нормально собеседника а в документации хрен чего полезного найдёшь 
                                                 // не считаю это готовым решением но в целом работает и не требует сторонних библиотек кроме самой библиотеки WTelegramClient
                                                 // кроме того что написано лучше нечего не менять оно ели работает но хотяб без ошибок
                                                 // максимум как-то переписать тригеры в лучшую версию но не более
                                                 // задумка прикольная реализация так себе
                                                 // [!] ВАЖНО https://my.telegram.org/ вот тут ввести свой номер и подтвердить - потом API development tools и там два верхних рядка заполнить и нажать create и сверху будет 
                                                 // api_id и api_hash их вставить в код вместо 123456 и "API_HASH"
    {

        using var client = new WTelegram.Client(
            123456, // ваш телеграмм айди вставить сюда без ковычек вместо 123456
            "API_HASH Ваш в этих ковычках просто вставить"
        );

        // Кэш пользователей и чатов.
        var knownUsers = new Dictionary<long, TL.User>();
        var knownChats = new Dictionary<long, TL.ChatBase>();

        var whitelist = new HashSet<long>
        {
            777000, 777001, 777002, 777003, 777004,
            777005, 777006, 777007, 777008, 777009,
            5541490142 // тут добавлять белый список типо каких не проверять 
        };

        string pattern =
            @"\b(?:деанон\w*|докс\w*|доксинг|доксингом|доксер\w*|" +
            @"пробив\w*|пробью|пробить|пробей|пробиваю|" +
            @"слив\w*|слить|солью|выложу|опубликую|разглашу|" + // тут лучше его чуть почистиить или вообще по другому сделать вышло слишком общее
            @"угроз\w*|шантаж\w*|вымогательств\w*|вымог\w*|" +
            @"убью|убить|убей|зарежу|застрелю|пристрелю|" +
            @"закопаю|задушу|сломаю|разобью|сожгу|подожгу|" +
            @"изобью|покалечу|прибью|уничтожу|найду|вычислю|" +
            @"отследю|отслежу|выследю|отомщу|пожалеешь|" +
            @"мразь|ублюдок|сука|тварь|урод)\b|" +

            @"(?:найду|найдём|найдем|вычислю|вычислим|пробью|" +
            @"пробьюсь|пробить|пробивать|доксну|деаноню|" +
            @"сливаю|слить|солью|выложу|опубликую|раскидаю|" +
            @"разошлю|узнаю|достану|найдём тебя|" +
            @"доберусь|приеду|прибуду).{0,100}" +
            @"(?:тебя|тебе|вас|адрес|дом|город|школ|родител|" +
            @"родствен|паспорт|телефон|номер|почт|аккаунт|" +
            @"айпи|ip\b|личн\w* данн\w*|фото|переписк|" +
            @"геолокац|местоположен|место жительств)|" +

            @"(?:убью|убить|зарежу|застрелю|пристрелю|" +
            @"сломаю|разобью|изобью|закопаю|сожгу|" +
            @"подожгу|задушу|покалечу|уничтожу|" +
            @"оторву|переломаю|взорву).{0,100}" +
            @"(?:тебя|тебе|вас|твою|твой|твоих|дом|" +
            @"семь|родител|маму|папу|брата|сестру)|" +

            @"(?:жди|оглядывайся|бойся|прячься|не спи|" +
            @"я знаю|мы знаем|я найду|я приеду|" +
            @"доберусь до|встречу тебя|увижу тебя|" +
            @"знаю где|знаю твой|знаю твою).{0,100}" +
            @"(?:тебя|тебе|вас|живёшь|живешь|живёт|" +
            @"живет|находишься|учишься|работаешь|" +
            @"адрес|дом|школ|подъезд|родител|семь|" +
            @"город|район|улиц)|" +

            @"(?:плати|заплати|переведи|скинь|отправь|" +
            @"дай денег|выкуп|откуп|деньги|крипт\w*|" +
            @"usdt|btc|bitcoin|ton).{0,100}" +
            @"(?:иначе|или|а то|не то|солью|удалю|" +
            @"опубликую|разошлю|выложу|распространю|" +
            @"родителям|друзьям|в сеть|в интернет|" +
            @"всем|тебя|данные|фото|видео|переписк)|" +

            @"(?:нюдс|нюдсы|интим\w*|слив\w* фото|" +
            @"слив\w* видео|компромат|переписк\w*|" +
            @"личн\w* фото|личн\w* видео).{0,100}" +
            @"(?:солью|выложу|разошлю|опубликую|" +
            @"покажу|отправлю|раскидаю|всем|родителям)|" +

            @"(?:посажу|накажу|заявлю|напишу заявлен|" +
            @"позвоню в полицию|найму людей|" +
            @"тебя найдут|тебе конец|тебе хана|" +
            @"пожалеешь|тебе пизда|ты труп)|" +

            @"(?:offer|deal|сделк\w*|аукцион\w*|лот|p2p|" +
            @"accept|confirm|transfer|receiv\w*|" +
            @"approv\w*|whitelist|mint|минт\w*|" +
            @"дроп\w*).{0,100}" +
            @"(?:подтверд\w*|перевод\w*|оплат\w*|" +
            @"получен\w*|отправлен\w*|ton|stars|" +
            @"nft|нфт|кошелёк|кошелек|токен\w*)";

        var reg = new Regex(
            pattern,
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        Console.Write("Введите номер телефона: ");
        string loginInfo = Console.ReadLine() ?? "";

        // Авторизация.
        while (client.User == null)
        {
            string result = await client.Login(loginInfo);

            switch (result)
            {
                case "verification-code":
                    Console.Write("Код из Telegram: ");
                    loginInfo = Console.ReadLine() ?? "";
                    break;

                case "phone-number":
                    Console.Write("Введите номер телефона: ");
                    loginInfo = Console.ReadLine() ?? "";
                    break;

                case "password":
                    Console.Write("Пароль двухэтапной аутентификации: ");
                    loginInfo = Console.ReadLine() ?? "";
                    break;

                default:
                    loginInfo = "";
                    break;
            }
        }

        Console.Clear();
        Console.WriteLine("Вход выполнен. Ожидание сообщений...");

        string backupFolder = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "BlockedUsersHistory"
        );

        Directory.CreateDirectory(backupFolder);

        var dialogs = await client.Messages_GetAllDialogs();
        dialogs.CollectUsersChats(knownUsers, knownChats);


        client.OnUpdates += async updates =>
        {
            try
            {

                if (updates is UpdateShortMessage usm &&
    !knownUsers.ContainsKey(usm.user_id))
                {
                    var difference = await client.Updates_GetDifference(
                        usm.pts - usm.pts_count,
                        usm.date,
                        0
                    );

                    difference.CollectUsersChats(knownUsers, knownChats);
                }


               
                updates.CollectUsersChats(knownUsers, knownChats);

                foreach (var update in updates.UpdateList)
                {
                    if (update is not UpdateNewMessage umsg ||
                        umsg.message is not Message msg)
                    {
                        continue;
                    }

                    string text = msg.message ?? "";

                    Console.WriteLine(
                        $"[DEBUG] Получено сообщение: {text}"
                    );

                    if (!reg.IsMatch(text))
                        continue;

                    
                    if (msg.From is not PeerUser sender)
                    {
                        Console.WriteLine(
                            "[DEBUG] Не удалось определить автора сообщения"
                        );
                        continue;
                    }

                    if (!knownUsers.TryGetValue(sender.user_id, out TL.User user))
                    {
                        Console.WriteLine(
                            $"[DEBUG] Пользователь {sender.user_id} отсутствует в кэше"
                        );
                        continue;
                    }

                    
                    if (user.id == client.UserId ||
                        whitelist.Contains(user.id))
                    {
                        continue;
                    }

                    Console.WriteLine(
                        $"[!] Совпадение: {user.first_name} " +
                        $"(ID {user.id}): {text}"
                    );

                    try
                    {
                        
                        var history = await client.Messages_GetHistory(
                            user,
                            limit: 100
                        );

                        string filename = Path.Combine(
                            backupFolder,
                            $"history_{DateTime.Now:yyyyMMdd_HHmmss}_{user.id}.txt"
                        );

                        using (var writer = new StreamWriter(
                            filename,
                            false,
                            Encoding.UTF8))
                        {
                            foreach (var item in history.Messages)
                            {
                                if (item is Message m)
                                {
                                    string senderName =
                                        m.from_id?.ID == client.UserId
                                            ? "Вы"
                                            : "Собеседник";

                                    writer.WriteLine(
                                        $"[{m.Date}] {senderName}: {m.message}"
                                    );
                                }
                            }
                        }

                        Console.WriteLine(
                            $"История сохранена: {filename}"
                        );

                        
                        await client.Contacts_Block(user);

                        await client.Messages_DeleteHistory(
              new InputPeerUser(user.id, user.access_hash),
             just_clear: false,
             revoke: true);


                        Console.WriteLine(
                            $"Пользователь {user.id} заблокирован."
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Ошибка обработки пользователя {user.id}: {ex.Message}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Ошибка обработки обновления: {ex.Message}"
                );
            }
        };

       
        Console.ReadLine();
    }
}
