CREATE TABLE public.drinks (
    id integer NOT NULL,
    name character varying(80) NOT NULL,
    description text NOT NULL,
    isbinge boolean DEFAULT false NOT NULL,
    strength integer DEFAULT 0 NOT NULL,
    haschaser boolean DEFAULT false NOT NULL,
    fortitudechange integer DEFAULT 0 NOT NULL,
    copies integer DEFAULT 1 NOT NULL,
    imagepath character varying(255),
    CONSTRAINT drinks_copies_check CHECK ((copies > 0))
);


ALTER TABLE public.drinks OWNER TO postgres;

CREATE SEQUENCE public.drinks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.drinks_id_seq OWNER TO postgres;

ALTER SEQUENCE public.drinks_id_seq OWNED BY public.drinks.id;

ALTER TABLE ONLY public.drinks ALTER COLUMN id SET DEFAULT nextval('public.drinks_id_seq'::regclass);

INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (1, 'Светлое пиво', 'Обычная выпивка: опьянение повышается на 1.', false, 1, false, 0, 1, '/card_01.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (2, 'Тёмное пиво', 'Обычная выпивка: опьянение повышается на 1.', false, 1, false, 0, 1, '/card_02.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (3, 'Вино', 'Обычная выпивка: опьянение повышается на 2.', false, 2, false, 0, 1, '/card_03.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (4, 'Светлое пиво с "прицепом"', 'Раскрой ещё одну карту выпивки и добавь её эффект. Крепость самой выпивки 1.', false, 1, true, 0, 1, '/card_04.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (5, 'Тёмное пиво с "прицепом"', 'Раскрой ещё одну карту выпивки и добавь её эффект. Крепость самой выпивки 1.', false, 1, true, 0, 1, '/card_05.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (6, 'Вино с "прицепом"', 'Раскрой ещё одну карту выпивки и добавь её эффект. Крепость самой выпивки 2.', false, 2, true, 0, 1, '/card_06.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (7, 'Эль "Дыхание дракона"', 'Обычная выпивка: опьянение повышается на 4.', false, 4, false, 0, 1, '/card_07.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (8, 'Эльфийское вино', 'Обычная выпивка: опьянение повышается на 3.', false, 3, false, 0, 1, '/card_08.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (9, 'Эльфийское вино с "прицепом"', 'Раскрой ещё одну карту выпивки и добавь её эффект. Крепость самой выпивки 3.', false, 3, true, 0, 1, '/card_09.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (10, 'Троллье пойло', 'Крепость выпивки 1, стойкость понижается на 1. Если вы Тролль, то вместо этого крепость 2.', false, 1, false, 0, 1, '/card_10.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (11, 'Помойная водица', 'Понижает стойкость на 1', false, 0, false, -1, 1, '/card_11.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (12, 'Орочья бурда', 'Cтойкость понижается на 2. Если вы Орк, то вместо этого крепость 2.', false, 2, false, -2, 1, '/card_12.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (13, 'Пьём по кругу!', 'Пьянка: раскрывается выпивка, и каждый игрок сразу получает эффект этой выпивки.', true, 0, false, 0, 1, '/card_13.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (14, 'Кто больше выпьет!', 'Пьянка: все игроки раскрывают выпивку из колоды, у кого она самая крепкая, получает по 1 золотому от каждого другого игрока. При ничьей раскрытие повторяют.', true, 0, false, 0, 1, '/card_14.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (15, 'Вода', 'Перерыв. Крепость выпивки 0.', false, 0, false, 0, 1, '/card_15.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (18, 'Взбодрись-ка!', 'Перерыв. Понижает опьянение на 1.', false, -1, false, 0, 1, '/card_18.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (19, 'До дна!', 'Пьянка: каждый игрок берёт карту выпивки из своей стопки "Выпей меня!"', true, 0, false, 0, 1, '/card_19.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (17, 'Святая вода', 'Повышает стойкость на 2.', false, 0, false, 2, 1, '/card_17.jpg');
INSERT INTO public.drinks (id, name, description, isbinge, strength, haschaser, fortitudechange, copies, imagepath) VALUES (16, 'Настойка Волшебников', 'Крепость выпивки 2, стойкость повышается на 2.', false, 2, false, 2, 1, '/card_16.jpg');


CREATE TABLE public.users (
    id integer NOT NULL,
    login character varying(50) NOT NULL,
    password character varying(100) NOT NULL,
    isadmin boolean DEFAULT false NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);

INSERT INTO public.users (id, login, password, isadmin) VALUES (2, 'guest', '84983c60f7daadc1cb8698621f802c0d9f9a3c3c295c810748fb048115c186ec', false);
INSERT INTO public.users (id, login, password, isadmin) VALUES (1, 'owner', '398120960e765be6e1efcee22da95f03d63e254d1ac33a30a50f5cd9cd903c7d', true);

CREATE TABLE public.characters (
    id integer NOT NULL,
    portraitpath character varying(60) NOT NULL,
    name character varying(60) NOT NULL,
    role character varying(80) NOT NULL,
    description text NOT NULL,
    pros text NOT NULL,
    cons text NOT NULL
);


ALTER TABLE public.characters OWNER TO postgres;

CREATE SEQUENCE public.characters_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.characters_id_seq OWNER TO postgres;

ALTER SEQUENCE public.characters_id_seq OWNED BY public.characters.id;

ALTER TABLE ONLY public.characters ALTER COLUMN id SET DEFAULT nextval('public.characters_id_seq'::regclass);

INSERT INTO public.characters (id, portraitpath, name, role, description, pros, cons) VALUES (3, '/character_icons/Gerki.png', 'Герки Ловкач', 'Плутоватый полурослик', 'В его карманах никогда не бывает пусто. Разбирается в ядах и механизмах, виртуозно бьёт исподтишка.', 'Так удачлив в азартных играх, что его считают шулером.', 'Он действительно мухлюет.');
INSERT INTO public.characters (id, portraitpath, name, role, description, pros, cons) VALUES (4, '/character_icons/Zot.png', 'Волшебник Зот', 'Волшебник и его питомец Пуки', 'Мог бы преподавать теорию магии в Университете Волшебства, если бы не питомец Пуки — бешеный кролик-алкаш.', 'Искусный карточный фокусник, арсенал заклинаний на все случаи.', 'Кролик реально поехавший.');
INSERT INTO public.characters (id, portraitpath, name, role, description, pros, cons) VALUES (2, '/character_icons/Fiona.png', 'Фиона Вспыльчивая', 'Воительница', 'Хамоватая воительница, отлично владеющая мечом, топором, кинжалом и всем, чем можно нанести колото-резаные раны.', 'Выстоит против самых могучих врагов и поглотит спиртное литрами.', 'Не может уследить за своим кошельком.');
INSERT INTO public.characters (id, portraitpath, name, role, description, pros, cons) VALUES (1, '/character_icons/Deirdre.png', 'Жрица Дейрдре', 'Жрица Богини', 'Защищает и исцеляет друзей, обрушивает праведный гнев на подземельных тварей, а при случае пускает в ход красоту.', 'Богиня защищает Дейрдре от зла.', 'Против алкоголя Богиня бессильна.');

CREATE TABLE public.cards (
    id integer NOT NULL,
    characterid integer NOT NULL,
    name character varying(80) NOT NULL,
    description text NOT NULL,
    isaction boolean DEFAULT false NOT NULL,
    issometimes boolean DEFAULT false NOT NULL,
    isalways boolean DEFAULT false NOT NULL,
    isgamble boolean DEFAULT false NOT NULL,
    ischeat boolean DEFAULT false NOT NULL,
    copies integer DEFAULT 1 NOT NULL,
    imagepath character varying(255),
    CONSTRAINT cards_check CHECK ((isaction OR issometimes OR isalways OR isgamble OR ischeat)),
    CONSTRAINT cards_copies_check CHECK ((copies > 0))
);

ALTER TABLE public.cards OWNER TO postgres;

CREATE SEQUENCE public.cards_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
ALTER SEQUENCE public.cards_id_seq OWNER TO postgres;

ALTER SEQUENCE public.cards_id_seq OWNED BY public.cards.id;

ALTER TABLE ONLY public.cards ALTER COLUMN id SET DEFAULT nextval('public.cards_id_seq'::regclass);

INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (1, 2, 'Отвали! Я полирую свой арсенал', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.', false, true, false, false, false, 1, 'Fiona/card_01.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (2, 2, 'Так вот что гремело в моём доспехе!', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Заплатите Золотом из кассы Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Fiona/card_02.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (3, 2, 'Я считаю, что ты сжульничал. У тебя есть два варианта...', 'Сыграйте эту карту, когда другой игрок играет карту «обмана», чтобы отменить её и закончить раунд «Азарта».
Вы побеждаете в раунде «Азарта».
Вы не можете играть эту карту, если уже покинули раунд «Азарта». (Но можете играть её в ответ на карту, принуждающую вас покинуть раунд.)', false, true, false, false, false, 1, 'Fiona/card_03.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (4, 2, 'Мне нравится этот город! Они дали мне скидку на ремонт!', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Заплатите Золотом из кассы Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Fiona/card_04.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (5, 2, 'Отвали! Я сортирую оружие по размеру.', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.', false, true, false, false, false, 1, 'Fiona/card_05.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (6, 2, 'Эй, ребята... Одолжите пару золотых на ремонт доспехов?', 'Каждый другой игрок платит вам по 1 Золотому.', true, false, false, false, false, 1, 'Fiona/card_06.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (7, 2, 'Нет ничего лучше, чем пинта жидкой храбрости!', 'Добавьте Опьянение от выпитой вами Выпивки к Стойкости, а не к вашему Опьянению.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Fiona/card_07.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (9, 2, 'Выпьем до дна, дружище!', 'Сыграйте эту карту после того, как другой игрок вскроет карту Выпивки. Он должен вскрыть ещё одну карту из стопки «Выпей меня!».', false, true, false, false, false, 1, 'Fiona/card_09.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (11, 2, 'Этой штукой я очищу кровь со своего меча!', 'Игнорируйте карту Выпивки. (Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Fiona/card_11.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (12, 2, 'Мы в трактире, а ты не пьёшь. Да что с тобой такое?!', 'Выберите другого игрока. Он должен вскрыть верхнюю карту Выпивки из своей стопки «Выпей меня!»', false, false, true, false, false, 1, 'Fiona/card_12.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (13, 2, 'Ну же, друг мой, садись и выпей со мной!', 'Поделитесь вскрытой вами Выпивкой с другим игроком. Каждый из вас получает по половине от полного эффекта (округляя вверх).
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Fiona/card_13.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (14, 2, 'Этой штукой я очищу ржавчину со своих доспехов!', 'Игнорируйте карту Выпивки.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Fiona/card_14.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (16, 2, 'Да на мне ни царапины!', 'Добавьте 2 к Стойкости.', false, false, true, false, false, 1, 'Fiona/card_16.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (18, 2, 'А затем я схватила Огра вот так.', 'Выберите другого игрока. Он теряет 3 ед. Стойкости.', true, false, false, false, false, 1, 'Fiona/card_18.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (21, 2, 'Так ты хочешь помериться силой?', 'Выберите другого игрока. Он теряет 1 ед. Стойкости.', true, false, false, false, false, 1, 'Fiona/card_21.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (10, 2, 'К счастью для меня, я надела доспех.', 'Игнорируйте карту «действие» или «иногда», воздействующую на вашу
Стойкость.', false, true, false, false, false, 2, 'Fiona/card_10.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (19, 2, 'Эй! Довольно шуток про бронелифчик!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 2, 'Fiona/card_19.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (15, 2, 'Будет ещё больнее, если сделать вот так!', 'Выберите другого игрока. Он теряет 1 ед. Стойкости.', true, false, false, false, false, 2, 'Fiona/card_15.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (37, 1, 'Судя по твоему виду, тебе это нужнее, чем мне!', 'Передайте вскрытую вами карту Выпивки другому игроку. Он получает все эффекты от неё вместо вас.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Deirdre/card_09.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (38, 1, 'Не будь глупцом!', 'Игнорируйте карту «действие» или «иногда», воздействующие на вашу Стойкость или Опьянение.', false, true, false, false, false, 1, 'Deirdre/card_10.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (39, 1, 'Я так не думаю!', 'Отменяет карту «иногда». Эта карта может быть Отменена только другой картой Я так не думаю!', false, true, false, false, false, 1, 'Deirdre/card_11.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (40, 1, 'Давайте выпьем за Богиню!', 'Каждый игрок (включая вас) вскрывает по карте из колоды Выпивки.
Если вскрывается карта «событие пьянки», сбросьте её (без применения её эффекта) и продолжайте вскрывать карты, пока не попадётся карта Выпивки.', true, false, false, false, false, 1, 'Deirdre/card_12.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (22, 2, 'Ох, думаю, Служанка решила, что это были её чаевые...', 'Сыграйте этой картой в любое время, чтобы закончить раунд «Азарта». Всё поставленное Золото отходит Трактиру.
Вы можете играть эту карту, даже если покинули раунд «Азарта». Вы не можете играть эту карту, когда раунд уже закончился, в ответ на карту повышения ставок или в ответ на карту, заканчивающую раунд.', false, true, false, false, false, 1, 'Fiona/card_22.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (23, 2, 'Чаевые Служанке!', 'Игрок по вашему выбору должен заплатить 1 Золотой Трактиру.', false, false, true, false, false, 1, 'Fiona/card_23.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (24, 2, 'Служанка считает, что пора прекратить играть с выпивкой.', 'Отменяет эффект воздействия карты «иногда» на Выпивку (Включая передачу Выпивки другими игроками, разделения и Игнорирования Выпивки).
Эту карту можно отменить только картой Я так не думаю!', false, true, false, false, false, 1, 'Fiona/card_24.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (26, 2, 'Играете? Я с вами!', 'Сыграйте этой картой во время вашего этапа Совершения действия, чтобы начать раунд «Азарта». Каждый игрок ставит по 1 Золотому. Сейчас Вы Контролируете раунд.', true, false, false, true, false, 5, 'Fiona/card_26.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (27, 2, 'Повышаю!', 'Даёт контроль над раундом «Азарта». Каждый игрок (включая вас) ставит на кон ещё по 1 Золотому.
Если вы не можете сделать ставку, то Другие игроки не делают ставку.
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 2, 'Fiona/card_27.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (28, 2, 'Выигрышная рука!', 'Даёт Контроль над раундом «Азарта».
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 2, 'Fiona/card_28.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (25, 2, 'Служанка, принеси ещё выпивки для моих друзей!', 'Сыграйте эту карту во время своего хода на этапе «Заказа выпивки».
Заплатите 1 Золотой Трактиру, чтобы купить 2 дополнительные Выпивки. Вы можете разместить их лицом вниз на стопке «Выпей меня!» любого игрока.', false, true, false, false, false, 2, 'Fiona/card_25.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (17, 2, 'Извини, просто у меня инстинкт целиться в голову...', 'Сыграйте эту карту сразу после того, как другой игрок нанёс урон вашей Стойкости. Он тоже теряет 2 ед. Стойкости.
Вы не можете сыграть эту карту, если играли карты для уменьшения или Игнорирования потери Стойкости.', false, true, false, false, false, 2, 'Fiona/card_17.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (29, 1, 'Сожалею, но сейчас я должна молиться...', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.', false, true, false, false, false, 1, 'Deirdre/card_01.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (30, 1, 'Моя Богиня даровала это!', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Заплатите Золотом из кассы Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Deirdre/card_02.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (31, 1, 'Жрица может достать деньги практически везде!', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Заплатите Золотом из кассы Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Deirdre/card_03.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (32, 1, 'Не сейчас! Этим людям интересна моя религия!', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.
- ИЛИ -
Игнорируйте карту Выпивки.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Deirdre/card_04.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (33, 1, 'Я сохраню эти деньги для бедных.', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.', false, true, false, false, false, 1, 'Deirdre/card_05.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (34, 1, 'Моя богиня узрела твой постыдный обман!', 'Сыграйте эту карту, когда другой игрок играет карту «обмана», чтобы Отменить её и закончить раунд «Азарта».
Вы побеждаете в раунде «Азарта».
Вы не можете играть эту карту, если уже покинули раунд «Азарта». (Но Можете играть её в ответ на карту, принуждающую вас покинуть раунд.)', false, true, false, false, false, 1, 'Deirdre/card_06.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (35, 1, 'Воду в Вино', 'Сыграйте эту карту после того, как другой игрок вскроет карту Выпивки. Помимо своего обычного эффекта, карта Выпивки добавляет 2 ќ Опьянению.', false, true, false, false, false, 1, 'Deirdre/card_07.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (36, 1, 'Вино в Воду', 'Уменьшает Опьянение от Выпивки на 2. (Опьянение от Выпивки не может опуститься ниже 0.)
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Deirdre/card_08.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (41, 1, 'Моя богиня заставила меня это сделать!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Deirdre/card_13.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (42, 1, 'Моя богиня исцелила меня.', 'Добавьте 2 к Стойкости.', false, false, true, false, false, 1, 'Deirdre/card_14.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (43, 1, 'О, нет! Похоже, что нарост на твоей руке это гниль мумии!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Deirdre/card_15.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (44, 1, 'Моя Богиня защищает меня!', 'Игнорируйте карту «действие» или «иногда», воздействующую на вашу Стойкость.', false, true, false, false, false, 1, 'Deirdre/card_16.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (46, 1, 'Я помолюсь об Исцелении', 'Выберите другого игрока. Он добавляет 2 к своей Стойкости и платит вам 1
Золотой.', true, false, false, false, false, 1, 'Deirdre/card_18.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (45, 1, 'Извини, иногда моё Исцеление просто не работает.', 'Выберите другого игрока. Он теряет 1 ед. Стойкости.', true, false, false, false, false, 1, 'Deirdre/card_17.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (47, 1, 'Массовое Исцеление!', 'Каждый игрок (включая вас) добавляет по 1 к Стойкости. Каждый другой игрок платит вам по 1 Золотому.', true, false, false, false, false, 1, 'Deirdre/card_19.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (48, 1, 'Такое не нравится моей Богине!', 'Сыграйте эту карту сразу после того, как другой игрок нанёс урон вашей Стойкости. Он тоже теряет 2 ед. Стойкости.
Вы не можете сыграть эту карту, если играли карты для уменьшения или Игнорирования потери Стойкости.', false, true, false, false, false, 1, 'Deirdre/card_20.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (49, 1, 'Я не такая жрица!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Deirdre/card_21.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (50, 1, 'Ох, думаю, Служанка решила, что это были её чаевые...', 'Сыграйте этой картой в любое время, чтобы закончить раунд «Азарта». Всё поставленное Золото отходит Трактиру.
Вы можете играть эту карту, даже если покинули раунд «Азарта». Вы не можете играть эту карту, когда раунд уже закончился, в ответ на карту повышения ставок или в ответ на карту, заканчивающую раунд.', false, true, false, false, false, 1, 'Deirdre/card_22.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (51, 1, 'Чаевые Служанке!', 'Игрок по вашему выбору должен заплатить 1 Золотой Трактиру.', false, false, true, false, false, 1, 'Deirdre/card_23.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (52, 1, 'Служанка считает, что пора прекратить играть с выпивкой.', 'Отменяет эффект воздействия карты «иногда» на Выпивку (Включая передачу Выпивки другими игроками, разделения и Игнорирования Выпивки). Эту карту можно отменить только картой Я так не думаю!', false, true, false, false, false, 1, 'Deirdre/card_24.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (53, 1, 'Служанка, принеси ещё выпивки для моих друзей!', 'Сыграйте эту карту во время своего хода на этапе «Заказа выпивки».
Заплатите 1 Золотой Трактиру, чтобы купить 2 дополнительные Выпивки. Вы можете разместить их лицом вниз на стопке «Выпей меня!» любого игрока.', false, true, false, false, false, 1, 'Deirdre/card_25.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (54, 1, 'Играете? Я с вами!', 'Сыграйте этой картой во время вашего этапа Совершения действия, чтобы начать раунд «Азарта». Каждый игрок ставит по [ Золотому. Сейчас вы Контролируете раунд.
- ИЛИ - Сыграйте этой картой во время раунда «Азарта» для Контроля над раундом.
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', true, false, false, true, false, 1, 'Deirdre/card_26.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (55, 1, 'Повышаю!', 'Даёт Контроль над раундом «Азарта». Каждый игрок (включая вас) ставит на кон ещё по 1 Золотому.
Если вы не можете сделать ставку, то Другие игроки не делают ставку.
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 1, 'Deirdre/card_27.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (56, 1, 'Выигрышная рука!', 'Даёт Контроль над раундом «Азарта».
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 1, 'Deirdre/card_28.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (57, 3, 'Ну и как же это воткнулось в твою спину?', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Gerki/card_01.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (58, 3, 'Ох! Я забыл обезвредить одну из ловушек!', 'Выберите другого игрока. Он теряет 3 ед. Стойкости.', true, false, false, false, false, 1, 'Gerki/card_02.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (59, 3, 'Свистнул чаевые!', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Заплатите Золотом из кассы Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Gerki/card_03.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (60, 3, 'Эй! Куда это смылся тот мелкий?', 'Игнорируйте карту «действие", иногда» или «всегда», воздействующую на вашу Стойкость, Опьянение или Золото.
- ИЛИ -
Игнорируйте карту Выпивки. (Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Gerki/card_04.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (61, 3, 'Я так не думаю!', 'Отменяет карту «иногда». Эта карта может быть Отменена только другой картой Я так не думаю!', false, true, false, false, false, 1, 'Gerki/card_05.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (62, 3, 'Ну... я не вижу на ней твоего имени.', 'Выберите другого игрока. Отнимите у него 1 Золотой.', false, false, true, false, false, 1, 'Gerki/card_06.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (65, 3, 'Это не целебный бальзам! Это контактный яд!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Gerki/card_9.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (63, 3, 'Спрятался в тени', 'Игнорируйте карту «действие» или «иногда», воздействующую на вашу Стойкость или Опьянение.', false, true, false, false, false, 1, 'Gerki/card_07.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (64, 3, 'Ты не видел мой яд? Он был в кружке, что здесь стояла...', 'Выберите другого игрока.  
Он теряет 3 ед. Стойкости.', true, false, false, false, false, 1, 'Gerki/card_8.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (66, 3, 'Хм... Я думаю, что здесь была допущена ошибка при дележе...', 'Каждый другой игрок платит вам по 1 Золотому.', true, false, false, false, false, 1, 'Gerki/card_10.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (67, 3, 'Клофелин улучшит вкус', 'Сыграйте эту карту после того, как другой игрок вскроет карту Выпивки. Помимо своего обычного эффекта, карта Выпивки добавляет 2 к Опьянению.', false, true, false, false, false, 1, 'Gerki/card_11.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (68, 3, 'Ах, эта монета? Хм... я нашёл её... под твоим стулом...', 'Выберите другого игрока. Отнимите у него 1 Золотой.', false, false, true, false, false, 1, 'Gerki/card_12.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (69, 3, 'Разлил по полу.', 'Игнорируйте карту Выпивки. (Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Gerki/card_13.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (70, 3, 'К счастью для меня, я сварил противоядие!', 'Игнорируйте карту Выпивки.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Gerki/card_14.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (71, 3, 'Стырил монету, пока никто не видел!', 'Возьмите 1 Золотой из ставок на раунд «Азарта».
Не оказывает других эффектов на раунд.
Мы можете играть этой картой, даже если покинули раунд «Азарта».', false, true, false, false, false, 1, 'Gerki/card_15.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (72, 3, 'Хм... Я знаю, ты думал, что победил, но...', 'Сыграйте эту карту, когда раунд «Азарта» закончен.
Всё поставленное Золото достаётся вам, как победителю.
Вы можете играть этой картой, Даже если уже покинули раунд «Азарта». Вы не можете играть этой картой, если была сыграна карта Ох, думаю, Служанка решила, что это были её чаевые...', false, true, false, false, false, 1, 'Gerki/card_16.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (73, 3, 'Я выигрываю... Честно-честно!', 'Даёт Контроль над раундом
«Азарта».', false, false, false, false, true, 1, 'Gerki/card_17.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (75, 3, 'Что это в твоём рукаве?', 'Даёт Контроль над раундом «Азарта». 
- А так же -
Выберите одного игрока в текущем раунде «Азарта», данная карта принуждает его покинуть раунд. Он не сможет больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное им Золото остаётся в игре.', false, false, false, false, true, 1, 'Gerki/card_19.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (77, 3, 'Ох, думаю, Служанка решила, что это были её чаевые...', 'Сыграйте этой картой в любое время, чтобы закончить раунд «Азарта». Всё поставленное Золото отходит Трактиру.
Вы можете играть эту карту, даже если покинули раунд «Азарта». Вы не можете играть эту карту, когда раунд уже закончился, в ответ на карту повышения ставок или в ответ на карту, заканчивающую раунд.', false, true, false, false, false, 1, 'Gerki/card_21.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (74, 3, 'Ой... Я уронил свои карты...', 'Даёт Контроль над раундом «Азарта».', false, false, false, false, true, 1, 'Gerki/card_18.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (79, 3, 'Служанка считает, что пора прекратить играть с выпивкой.', 'Отменяет эффект воздействия карты «иногда» на Выпивку (Включая передачу Выпивки другими игроками, разделения и Игнорирования Выпивки).
Эту карту можно отменить только картой Я так не думаю!', false, true, false, false, false, 1, 'Gerki/card_23.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (80, 3, 'Служанка, принеси ещё выпивки для моих друзей!', 'Сыграйте эту карту во время своего хода на этапе «Заказа выпивки».
Заплатите 1 Золотой Трактиру, чтобы купить 2 дополнительные Выпивки. Вы можете разместить их лицом вниз на стопке «Выпей меня!» любого игрока.', false, true, false, false, false, 1, 'Gerki/card_24.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (81, 3, 'Играете? Я с вами! «действие - азарт»', 'Сыграйте этой картой во время вашего этапа Совершения действия, чтобы начать раунд «Азарта». Каждый игрок ставит по 1 Золотому. Сейчас вы Контролируете раунд.
- ИЛИ - Сыграйте этой картой во время раунда «Азарта» для Контроля над раундом.
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', true, false, false, true, false, 1, 'Gerki/card_25.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (82, 3, 'Повышаю!', 'Даёт Контроль над раундом «Азарта». Каждый игрок (включая вас) ставит на кон ещё по 1 Золотому.
Если вы не можете сделать ставку, то Другие игроки не делают ставку.
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 1, 'Gerki/card_26.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (76, 3, 'Пять карт одного достоинства! Это значит что я выиграл?', 'Даёт Контроль над раундом «Азарта».', false, false, false, false, true, 1, 'Gerki/card_20.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (83, 3, 'Выигрышная рука!', 'Даёт Контроль над раундом «Азарта».
Вы не можете играть эту карту, если последней картой Контроля была Выигрышная рука.', false, false, false, true, false, 1, 'Gerki/card_27.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (78, 3, 'Чаевые Служанке!', 'Игрок по вашему выбору Должен заплатить 1 Золотой Трактиру.', false, false, true, false, false, 1, 'Gerki/card_22.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (20, 2, 'Кто вякнул, что я не Леди?', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 2, 'Fiona/card_20.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (8, 2, 'Я так не думаю!', 'Отменяет карту «иногда». Эта карта может быть Отменена только другой картой Я так не думаю!', false, true, false, false, false, 1, 'Fiona/card_08.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (84, 4, 'Не сейчас. Я медитирую.', 'Сыграйте эту карту, когда вы должны Делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре. 
- ИЛИ -
Игнорируйте карту Выпивки.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Zot/card_01.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (85, 4, 'Ох, Барменша! Кажется с Пуки случилась авария!', 'Сыграйте эту карту, когда вы должны делать ставку, чтобы не платить и покинуть раунд «Азарта». Вы не сможете больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное вами Золото остаётся в игре.
- ИЛИ -
Игнорируйте карту Выпивки. (Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.) В любом случае вы должны заплатить 1 Золотой за полотенце, чтобы вытереть «несчастный случай» Пуки.', false, true, false, false, false, 1, 'Zot/card_02.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (86, 4, 'Превращаю орехи в золото.', 'Сыграйте эту карту, когда вы теряете Золото. (Включая плату Золотом, ставку Золотом или отъём вашего Золота.)
Вы платите Золотом из Трактира, а не из своего Тайника.', false, true, false, false, false, 1, 'Zot/card_03.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (87, 4, 'Пуки! Перестать Заглядывать в чужие карты!', 'Даёт Контроль над раундом «Азарта».', false, false, false, false, true, 1, 'Zot/card_04.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (89, 4, 'Вы только гляньте! Это же Король-лич!', 'Даёт Контроль над раундом «Азарта».', false, false, false, false, true, 1, 'Zot/card_06.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (90, 4, 'О нет! Опять! Пуки впал в пьяное буйство!', 'Каждый игрок (кроме вас) теряет по 1 ед. Стойкости.', true, false, false, false, false, 1, 'Zot/card_07.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (91, 4, 'Я же говорил не трогать мою волшебную палочку!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Zot/card_08.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (92, 4, 'Что это в твоём рукаве?', 'Даёт контроль над раундом «Азарта».
- А так же -
Выберите одного игрока в текущем раунде «Азарта», данная карта принуждает его покинуть раунд. Он не сможет больше играть карты «азарта» или «обмана» до конца этого раунда. Всё уже поставленное им Золото остаётся в игре.', false, false, false, false, true, 1, 'Zot/card_09.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (93, 4, 'Я же просил не отвлекать меня!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Zot/card_10.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (94, 4, 'Сидеть, Пуки!', 'Выберите другого игрока. Он теряет 1 ед. Стойкости.', true, false, false, false, false, 1, 'Zot/card_11.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (95, 4, 'Осторожнее! Не наступи на Пуки!', 'Выберите другого игрока. Он теряет 2 ед. Стойкости.', true, false, false, false, false, 1, 'Zot/card_12.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (96, 4, 'Нет, Пуки! Это мой друг!', 'Сыграйте эту карту сразу после того, как другой игрок нанёс урон вашей Стойкости. Он тоже теряет 2 ед. Стойкости.
Вы не можете сыграть эту карту, если играли карты для уменьшения или Игнорирования потери Стойкости.', false, true, false, false, false, 1, 'Zot/card_13.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (97, 4, 'Пуки думает, что тебя мучает жажда...', 'Выберите другого игрока. Он должен вскрыть верхнюю карту Выпивки из своей стопки «Выпей меня!»', false, false, true, false, false, 1, 'Zot/card_14.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (98, 4, 'Звёзды говорят "Нет!"', 'Игнорируйте карту «действие», «иногда» или «всегда», воздействующую на вашу Стойкость, Опьянение или Золото.', false, true, false, false, false, 1, 'Zot/card_15.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (99, 4, 'Телепортирую выпивку!', 'Передайте вскрытую вами карту Выпивки другому игроку. Он получает все эффекты от неё вместо вас.
(Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Zot/card_16.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (100, 4, 'Плохой Пуки! Не смей пить это!', 'Игнорируйте карту Выпивки. (Вы можете посмотреть карту Выпивки прежде, чем решите сыграть эту карту.)', false, true, false, false, false, 1, 'Zot/card_17.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (101, 4, 'Сейчас ты меня видишь... а теперь нет!', 'Игнорируйте карту «действие» или «иногда», воздействующую на вашу Стойкость.', false, true, false, false, false, 1, 'Zot/card_18.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (102, 4, 'Узрите моё могущество! Я вытащил кролика из шляпы!', 'Каждый другой игрок платит вам по 1 Золотому.', true, false, false, false, false, 1, 'Zot/card_19.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (103, 4, 'Мои руки пусты! Вуаля! Что это у тебя за ухом?', 'Выберите другого игрока. Отнимите у него 1 Золотой.', false, false, true, false, false, 1, 'Zot/card_20.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (104, 4, 'Я так не думаю!', 'Отменяет карту «иногда». Эта карта может быть Отменена только другой картой Я так не думаю!', false, true, false, false, false, 1, 'Zot/card_21.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (105, 4, 'Ох, думаю, Служанка решила, что это были её чаевые...', 'Сыграйте этой картой в любое время, чтобы закончить раунд «Азарта». Всё поставленное Золото отходит Трактиру.', false, true, false, false, false, 1, 'Zot/card_22.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (106, 4, 'Чаевые Служанке!', 'Игрок по вашему выбору Должен заплатить 1 Золотой Трактиру.', false, false, true, false, false, 1, 'Zot/card_23.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (107, 4, 'Служанка считает, что пора прекратить играть с выпивкой.', 'Отменяет эффект воздействия карты «иногда» на Выпивку (Включая передачу Выпивки другими игроками, разделения и Игнорирования Выпивки). 
Эту карту можно отменить только картой Я так не думаю!', false, true, false, false, false, 1, 'Zot/card_24.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (108, 4, 'Служанка, принеси ещё выпивки для моих друзей!', 'Сыграйте эту карту во время своего хода на этапе «Заказа выпивки».
Заплатите 1 Золотой Трактиру, чтобы купить 2 дополнительные Выпивки. Вы можете разместить их лицом вниз на стопке «Выпей меня!» любого игрока.', false, true, false, false, false, 1, 'Zot/card_25.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (109, 4, 'Играете? Я с вами!', 'Сыграйте этой картой во время вашего этапа Совершения действия, чтобы начать раунд «Азарта». Каждый игрок ставит по 1 Золотому. Сейчас вы Контролируете раунд.', true, false, false, true, false, 1, 'Zot/card_26.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (110, 4, 'Повышаю!', 'Даёт контроль над раундом «Азарта». Каждый игрок (включая вас) ставит на кон ещё по 1 Золотому.
Если вы не можете сделать ставку, то Другие игроки не делают ставку.', false, false, false, true, false, 1, 'Zot/card_27.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (111, 4, 'Выигрышная рука!', 'Даёт Контроль над раундом «Азарта».
Вы не можете играть эту карту, если последней картой контроля была Выигрышная рука.', false, false, false, true, false, 1, 'Zot/card_28.jpg');
INSERT INTO public.cards (id, characterid, name, description, isaction, issometimes, isalways, isgamble, ischeat, copies, imagepath) VALUES (88, 4, 'В этот раз используем мои игральные кости.', 'Даёт Контроль над раундом «Азарта».', false, false, false, false, true, 1, 'Zot/card_05.jpg');
