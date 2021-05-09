$("input#username").focus(function(){
	$("input#username").parent().addClass("usernamehover");
});
$("input#username").blur(function(){
	$("input#username").parent().removeClass("usernamehover");
});
$("input#password").focus(function () {
    $("input#password").parent().addClass("passwordhover");
});
$("input#password").blur(function () {
    $("input#password").parent().removeClass("passwordhover");
});