//Menu dropdown//
/**
  * NAME: Bootstrap 3 Triple Nested Sub-Menus
  * This script will active Triple level multi drop-down menus in Bootstrap 3.*
  */
 
if($(window).width() < 767)
{
   $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function(event) {
    // Avoid following the href location when clicking
    event.preventDefault(); 
    // Avoid having the menu to close when clicking
    event.stopPropagation(); 
    // Re-add .open to parent sub-menu item
    $(this).parent().addClass('open');
    $(this).parent().find("ul").parent().find("li.dropdown").addClass('open');
});

} else {
   // change functionality for larger screens
}


// Dropdown submenu inside screen code//
$('.dropdown-menu').parent().hover(function() {
    var menu = $(this).find("ul");
    var menupos = $(menu).offset();

    if (menupos.left + menu.width() > $(window).width()) {
        var newpos = -$(menu).width();
        menu.css({ left: newpos });    
    }
});

//End menu dropdown//    



//Menu Fixed when user do srcoll//
$('#nav').affix({
});
//End Menu Fixed//



//Font Size Property + -//
$(document).ready(function() {
  $('#incfont').click(function(){   
        curSize= parseInt($('.container').css('font-size')) + 1;
  if(curSize<=15)
        $('.container').css('font-size', curSize);
        }); 
  $('#decfont').click(function(){   
        curSize= parseInt($('.container').css('font-size')) - 1;
  if(curSize>=13)
        $('.container').css('font-size', curSize);
        });
 });
//End of Font Size Property + -//



//Css change on click//
$("#GreenCss").click(function(){
    $('head').append('<link href="css/green.css" rel="stylesheet" id="green" />');
});
$("#GreenCss").delegate("p", "click", function(){
    alert('you clicked me again!');
});
$("#RevertCss").click(function(){
	$('#green').remove();
});
//End of Css change on click//


// bootstrap carousel slide on hover stop issue //
$('.carousel').carousel({
    pause: "false"
});
// End of bootstrap carousel slide on hover stop issue //


