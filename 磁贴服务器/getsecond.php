<?php
$html = file_get_contents('https://angry.im/l/life');
$allSeconds = intval($html);
$dd = $allSeconds / 60 / 60 / 24 % 9999999;
$hh = $allSeconds / 60 / 60 % 24;
$mm = $allSeconds / 60 % 60;
$ss = $allSeconds % 60;
if(date("m/d")=="08/17"){$result = 75;}else{$result = rand(1,74);}

//随机输出一句长者语录
function random_str(){
$poems="我们党要始终代表中国先进社会生产力的发展要求，代表中国先进文化的前进方向，代表中国最广大人民的利益。
我们党要始终代表中国先进生产力的发展要求——就是党的理论、路线、纲领、方针、政策和各项工作，必须努力符合生产力发展的规律，体现不
隐患险于明火，防范胜于救灾，责任重于泰山。
朱镕基同志啊，你举行记者招待会是以中央提名的市长候选人名义，所以我认为朱镕基同志讲这句话从原则上讲没有错。
我们俩都没有坏心，就是口直心快。讲话，上海人讲 “吃相难看”，就是说话 “吃相难看”。
由于您的到来，上海的天气好转了。
你我一看（指姜昆）我就知道你的名字，你今天晚上真是表演得可是淋漓尽致。这相声演员也要讲究基本功。
（对青年京剧演员雷英）唱京剧。唱青衣还是唱什么？
（对胡松华）哎呀呀，这这，这专门唱民歌的，蒙古民歌。
如联合声明所确定的，1997 年中国恢复对香港行使主权后将实行 “一国两制”，香港将继续实行资本主义制度，实行 “一国两制” 是长期的。
一个继续坚持社会主义制度的中国大陆有助于香港的稳定和繁荣。
在香港过渡期间，中国政府无意干预港英政府的日常行政管理；但希望港英政府多从保持香港今后的长期稳定和繁荣方面考虑。
早日实现两岸直接“三通”，不仅是广大台胞、特别是台湾工商业者的强烈呼声，而且成为台湾未来经济发展的实际需要。
两岸直接通邮、通航、通商，是两岸经济发展和各方面交往的客观需要，也是两岸同胞利益之所在，完全应当采取实际步骤加速实现直接“三通”
在我祝酒之前，我去弹一曲《Aloha'Oe》这个 Guitar，请州长夫人演唱！
中国政府解决台湾问题的基本方针，是 “和平统一、一国两制”。
我今天非常激动。我回忆起两年前你到纽约，这样一个长途去看我，我实在是不敢当。
根据中国的优秀的历史传统，你是我的老师，我今天见到你，我又想起 51 年前，就是 46 年你在教我 operational calculus（运算微积分）。
记忆最深的，你上堂讲课没有带书，没有带讲义，全部在你的脑海里，而且我们还跟不上，这一点实在了不起。
你不仅是电机博士，而且你又是戏剧学家，又是诗人，而且呢我们的王师母啊，那个书法是好极了，又能够丹青，画画可以叫丹青吧，顾老师？
记得我在高中读书时，老师给我们讲微积分，第一课就是讲《庄子》中的 “一尺之棰，日取其半，万世不竭”，很形象地使我建立起极限的概念。
中国古人就已认识到事物的发展变化是无限的，说明我们的先人对自然界的认识已达到相当的水平。
许多中国人参加了美国的建设事业，不少美国人同情和帮助过中国的民族解放事业。他们的动人事迹，我们永远记在心中。
我很欣赏哈佛校门上的一段话：“为增长智慧走进来，为服务祖国和同胞走出去。” 中国青年也把 “胸怀祖国、服务人民” 作为自己的座右铭。
虽然我已经七十一岁了，但是呢我的耳朵还是很尖锐...外面高音喇叭的声音。那么但是，我想我唯一的办法，就是我的声音要比它还高。
广大军民，努力地奋战，与洪水搏斗。我们的军队要发扬不怕疲劳，不怕艰险，连续作战的精神。
现在已经面临着我们长江抗洪抢险的决战性的时刻...我们要坚决地坚持到底，坚持奋战，坚持再坚持！我们就一定能够取得最后的胜利。
我们中华民族是有着很强的凝聚力，任何的困难都压不倒我们。中国人民是不可战胜的！
有利于中日友好的事，要竭尽全力去做。不利于中日友好的事，绝不要去做！
所有人，不论长幼都必须说英语。
世界是丰富多彩的。如同宇宙间不能只有一种色彩一样，世界上也不能只有一种文明、一种社会制度、一种发展模式、一种价值观念。
时代在前进，事业在发展，党和国家对各方面人才的需求必然越来越大。要抓紧做好培养、吸引和用好各方面人才的工作。
为了实现这个宏伟目标（全面建成小康社会），中国需要一个长期和平稳定的周边和国际环境。
我们反对一切形式的恐怖主义，最近一个时期的实际行动，就充分说明了我们的态度。
我坚信，我们党的事业必定会不断地取得更大的胜利！
人呐，不管你坐在什么位置，他不能不回忆起曾经培养过的母校，他不可能忘掉，这个我看中外一律。
（江泽民题字时）你们给我搞的这本东西啊，Excited！
因为他们这里洋文好的人多得很哪
人呐就都不知道，自己就不可以预料。一个人的命运啊，当然要靠自我奋斗，但是也要考虑到历史的行程。
我绝对不知道，我作为一个上海市委书记怎么把我选到北京去了，所以邓小平同志跟我讲话，说中央都决定啦，你来当总书记，我说另请高明吧
我实在我也不是谦虚，我一个上海市委书记怎么到北京来了呢？但是呢，小平同志讲 “大家已经研究决定了”
所以后来我就念了两首诗（原话如此），叫 “苟利国家生死以，岂因祸福避趋之”，那麼所以我就到了北京。
到了北京我干了这十几年也没有什么别的，大概三件事
如果说还有一点什么成绩就是军队一律不得经商！这个对军队的命运有很大的关系。
但这些都是次要的，我主要的我就是三件事情，很惭愧，就做了一点微小的工作，谢谢大家。
语言是人类交流交往的重要工具。加强同各国人民的交流交往，需要在学好祖国语言的同时认真学习外语，领导干部尤其要以身作则。
领导干部如果能够直接用外语进行基本交流，都来做促进相互了解工作，就会产生很好的效果。
学习外语，贵在坚持。学习和掌握一门外语不容易，需要付出艰苦努力。只要把握规律，坚持不懈，日积月累，就一定能不断有所收获。
不来这个海南名山遗憾了，这么好的风景名胜海南要大力宣传，北京也要大力宣传，我回北京也为你们宣传宣传，以后这山就人山人海了。
江泽民到此，不虚此行。
中国宪法和法律明确地规定，公民有思想言论，信仰等方面的自由权利，政府依法保障公民依法行使这些权力。
在美国里面，监狱里面的犯人当中也会有许多的不同政见或者不同的宗教观点。
毋庸讳言，中美之间存在一些分歧，但可以通过讨论和会谈，求同存异...扩大共识，发展合作，共创未来的目的。
每个国家的民主自由，它不是一个绝对的概念，它必须要跟这个国家的发展，经济发展水平，历史文化传统，跟整个老百姓的教育水平相联系。
Я помню чудное мгновенье…（我还记得那些精彩的瞬间）
Великийпоэт，национальнаягордость！（伟大的诗人，国家的骄傲）
我，衷心地祝愿，明年这一个鸡年，能够风调雨顺，能够，我们的国家，走向繁荣昌盛，使得我们全体的人民，能够生活幸福。
为什么坦克会停下来？是那孩子截停坦克吗？是因为坦克，坦克里的人不想碾过拦在前面的人。
我对于中国的十二亿人口，我要把他管理好，要保证他吃得饱穿的好穿得暖，这不是一件容易的事。
西藏绝对是中国领土不可分割的一部分，因此这个问题是中国的内政。
I'm sorry. I am an electrical power engineer. （我很抱歉，我是一名电机工程师）。
我们需要有所选择，我们希望尽可能地限制对中国发展无用的信息。
媒体，应该是党的喉舌。我们的确有新闻自由，但是这种自由应该从属并服从于国家的利益。
我认为部队经商是一个腐蚀剂。因为历史经验已经告诉我们，任何一个国家如果军队经商以后，没有一个不腐败的，最后必然是涣散了军心。
我刚才跟你讲，我们对台湾的方针是一贯的：和平统一，一国两制但如果有干涉主义的势力干预，当然我们不会放弃使用武力。
中国人民是从来都不信邪的。
吼啊！
彭定康说的就是真的啦？你们媒体千万要注意啊，不要 “见着风，是得雨” 啊。接到这些消息，你媒体本身也要判断，明白意思吗？
假使这些完全…… 无中生有的东西，你再帮他说一遍，你等于…… 你也等于…… 你也有责任吧？
没有任何（内定、钦点）的意思。还是按照香港的…… 按照基本法、按照选举的法——去产生……
你…… 刚才你问我啊，我可以回答你一句 “无可奉告”，那你们又不高兴，那怎么办？
我讲的意思不是我是钦点他当下一任。你问我不支…… 支持不支持，我是支持的。我就明确地给你告诉这一点。
我觉得你们啊，你们…… 我感觉你们新闻界还要学习一个，你们非常熟悉西方的这一套 value。
你们毕竟还 too young，明白这意思吧。我告诉你们我是身经百战了，见得多了！啊，西方的哪一个国家我没去过？
媒体他们——你…… 你们要知道，美国的华莱士，那比你们不知道高到哪里去了。啊，我跟他谈笑风生！
所以说媒体啊，要…… 还是要提高自己的知识水平！懂我的意思——识得唔识得啊？
你们真的…… 我以为…… 遍地…… 你们有一个好，全世界跑到什么地方，你们比其他的西方记者啊，跑得还快。
但是呢，问来问去的问题啊，都 too simple，啊，sometimes naïve!懂了没有啊？
我很抱歉，我今天是作为一个长者给你们讲的。我不是新闻工作者，但是我见得太多了，我…… 我有这个必要告诉你们一点，人生的经验。
我刚才呢…… 我刚才我很想啊，就是我每一次碰到你们我就讲中国有一句话叫「闷声大发财」，我就什么话也不说。這是最好的！
但是我想，我见到你们这样热情啊，一句话不说也不好。所以你刚才你一定要——在宣传上将来如果你们报道上有偏差，你们要负责的。
我没有说要钦定，没有任何这个意思。但是你一定要非得要问我对董先生支持不支持。我们不支持他？他现在是当特首，我们怎么能不支持特首？
诶，连任也要按照香港的法律啊，对不对？要要…… 要按照香港的…… 当然我们的决定权也是很重要的。
你们啊，不要想…… 喜欢…… 弄个大新闻，说现在已经钦定了，再把我批判一番。
你们啊，naïve!
I'm angry!我跟你讲啊，你们这样子啊，是不行的！
我今天算是得罪了你们一下！";
$poems=explode("\n",$poems);
return $poems[rand(0,count($poems)-1)];
}
$resultstr = random_str();
?>
<tile>
  <visual version="2" branding="logo" displayName="+1s">
    <binding template="TileSquare150x150PeekImageAndText01" fallback="TileSquarePeekImageAndText01">
      <image id="1" src="ms-appx:///Assets/pic/<?php echo $result;?>.png" alt="alt text"/>
      <text id="1">时间众筹总计</text>
      <text id="2"><?php echo $dd.'天';?></text>
      <text id="3"><?php echo $hh.'小时';?></text>
      <text id="4"><?php echo $mm.'分钟';?></text>
    </binding>
    <binding template="TileWide310x150Text09" fallback="TileWideText09">
      <text id="1"><?php echo '时间众筹总计：'.$dd.'天'. $hh.'小时'.$mm.'分钟';?></text>
      <text id="2">长者语录：<?php echo $resultstr;?></text>
    </binding>
    <binding template="TileLarge">
      <text hint-style="title">时间众筹总计：</text>
      <text hint-style="base"><?php echo $dd.'天'. $hh.'小时'.$mm.'分钟';?></text>
      <text hint-wrap="true" hint-style="baseSubtle">长者语录：<?php echo $resultstr;?></text>
    </binding>
  </visual>
</tile>