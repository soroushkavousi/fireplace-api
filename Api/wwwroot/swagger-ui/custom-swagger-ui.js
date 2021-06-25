//$(document).ready(function () {
//    alert('ok');
//});​

$(document).ready(function () {
    console.log("ready!");
});

function codeAddress1() {
    //alert('ok1');
}

function testFunction(event) {
    //alert('ok2');
    var renderedMarkdownList = document.getElementsByClassName("renderedMarkdown");
}

//document.onload = codeAddress1();
//window.onload = codeAddress2(); 

document.addEventListener("DOMContentLoaded", testFunction);