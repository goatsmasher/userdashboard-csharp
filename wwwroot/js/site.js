$(document).ready(function () {
    
   $('#generate').submit(function(event){
           event.preventDefault();
           var $form = $(this);
           url = $form.attr("action");
           $.post(url, $form.serialize(), function(data){
               var newHTML = $(data).find('.response')[0];
               $('.response').html(newHTML);
           })
       })

});


//need to implement some ajax calls for the wall portion of this
//and also get some cool popups going on
//also put in my error models for a failed login, a taken email