let users = []; //array bilgileri saklicak

function login() {
    // document.querySelector fonks. htmlden belirli öge seçmek için .login sınıfından input öğesindeki text kısmı
    let username = document.querySelector('.login input[type="text"]').value;
    let password = document.querySelector('.login input[type="password"]').value;

    
    // Varsayılan giriş bilgileri
    if (username === 'admin' && password === 'admin') {
        // Giriş başarılıysa table.html sayfasına yönlendir
        window.location.href = 'table.html';
    } else {
        alert('Yanlış kullanıcı adı veya şifre!');
    }

    // Giriş bilgilerini kaydetmek
    users.push({ username: username, password: password });

    console.log(users);


    document.querySelector('.login').style.display = 'none'; //.login sınıfına sahip öge seçildi görünürlük gizllendi
    document.querySelector('.arrow').style.display = 'none';

    
    document.querySelector('.gif-background').style.display = 'block';
}
function startClock() {
    const clockElement = document.createElement('div');    //html sayfasında div oluştur ,saati göstercek
    clockElement.style.position = 'absolute';
    clockElement.style.top = '10px'; //üst kenardan 10 piksel aşağı
    clockElement.style.right = '10px';//sağ kenardan 10 içeri
    clockElement.style.color = 'white';
    clockElement.style.fontFamily = 'TokyoNight, sans-serif';
    clockElement.style.backgroundColor = '#333'; //koyu gri
    clockElement.style.padding = '10px';
    clockElement.style.borderRadius = '5px';
    clockElement.style.border = '1px solid #ddd'; //ddd açık gri
    clockElement.style.fontSize = '16px';
    clockElement.style.boxSizing = 'border-box';
    clockElement.style.zIndex = '9999';//her elemandan üstte
    document.body.appendChild(clockElement); //clockelemnt öğesi body kısmına ekle 

    function updateClock() {
        const now = new Date();  // new Date() şu anki tarih ve saati alır 
        clockElement.textContent = now.toLocaleTimeString();  //now.toLocaleTimeString() now değişkenindeki zamanı saat.dakika.saniye formatı
        //clockEelemnt.texconnet metni günceller 
    }

    setInterval(updateClock, 1000); //10000 milisaniyede fonksiyon çalışsın
} 


let formsHidden = false; //form görülcek
document.addEventListener('keydown', function(event) { //gpt ye herhangi bir tuşa basıldığında sayfada değişikler olmsını istiyorum kemik kod yapısı nedir diye sordum ve h durumunu kendim entegre ettim
// ( fonksiyonu belirli bir olay(event) gerçekleştiğinde işlem yapılması için)
// keydown parametresi tuşa basıldığı an çalışması için ---> tuş bırakıldığında keyup
//                                                           yazılabilir karakter algıla keypress (önerilmiyo)
//                                                           fare tıklanınca click
//event tarayıcı otamatik bu nesneyi oluşturuyo. event.key de hangi tuş
//                                               event.code klavyedeki tuşun kodunu "keyH"  veya "Enter"  
//                                               event.type olayın türünü döndürür  click ,keyup
//                                               event.target  olayın gerçekleştiği html elemaını 
//                                               event.shiftKey shift tuşuna basılı olup olmadığını kontrol eder tru-false
if (event.key === 'h' || event.key === 'H') {   
    let login = document.querySelector('.login'); //login (sınıfının) çerçevesindeki ögeleri login değişkenkine ata
    let arrow = document.querySelector('.arrow');

    if (formsHidden) {
        login.style.visibility = 'visible'; //login çerçevesini görünür yap
        arrow.style.visibility = 'visible'; 
    } else {
        login.style.visibility = 'hidden'; //login'i gizle ama sayfada yer kaplamaya devam etsin
        arrow.style.visibility = 'hidden'; 
    }
    formsHidden = !formsHidden;
}
});
window.onload = function() { //saat gözükmüyordu gpt ile bu kodu aldım
    startClock();
};