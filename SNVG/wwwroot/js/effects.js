function sticky_header() {

    "use strict";
    $(window).scroll(function() {
        if ($(this).scrollTop() > 27){  
            $('.header-bottom').addClass("sticky");
        }
        else{
            $('.header-bottom').removeClass("sticky");
        }
        if ($(this).scrollTop() > 150) {
            $('.scrollToTop') .addClass('left') .removeClass('right');
        } else {
            $('.scrollToTop') .addClass('right') .removeClass('left');
        }
    });
}
function gototop() {
    "use strict";
    //Click event to scroll to top
    $('.scrollToTop').on("click", function(){
        $('html, body').animate({scrollTop : 0},800);
        return false;
    });
}
jQuery(document).ready(function($) {
    sticky_header();
        gototop();
});

 $(".rgst").click(function(){
       $("#tab10").addClass("active");
       $("#tab10").addClass("in");
       $("#tab11").removeClass("active");
       $("#tab11").removeClass("in");
   });

 $(".lgin").click(function(){
       $("#tab11").addClass("active");
       $("#tab11").addClass("in");
       $("#tab10").removeClass("active");
       $("#tab10").removeClass("in");
   });


 


