(function($) {
  'use strict';
  $(function() {
    var body = $('body');
      var footer = $('.footer');
    var current = location.pathname.split("/").slice(-2)[0].replace(/^\/|\/$/g, '');
    $('.navbar.horizontal-layout .nav-bottom .page-navigation .nav-item').each(function() {
      var $this = $(this);
      if (current === "") {
        //for root url
        if ($this.find(".nav-link").attr('href').indexOf("Home/Index") !== -1) {
          $(this).find(".nav-link").parents('.nav-item').last().addClass('active');
          $(this).addClass("active");
        }
      } else {
        //for other url
        if ($this.find(".nav-link").attr('href').indexOf(current) !== -1) {
          $(this).find(".nav-link").parents('.nav-item').last().addClass('active');
          $(this).addClass("active");
        }
      }
    })

    $(".navbar.horizontal-layout .navbar-menu-wrapper .navbar-toggler").on("click", function() {
      $(".navbar.horizontal-layout .nav-bottom").toggleClass("header-toggled");
    });

    // Navigation in mobile menu on click
    var navItemClicked = $('.page-navigation >.nav-item');
    navItemClicked.on("click", function(event) {
      if(window.matchMedia('(max-width: 991px)').matches) {
        if(!($(this).hasClass('show-submenu'))) {
          navItemClicked.removeClass('show-submenu');
        }
        $(this).toggleClass('show-submenu');
      }        
    })
    

    //checkbox and radios
    $(".form-check .form-check-label,.form-radio .form-check-label").not(".todo-form-check .form-check-label").append('<i class="input-helper"></i>');

      body.tooltip({ selector: '[data-toggle="tooltip"]' });
  });

    function InitModal() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action").on('shown.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new InitModal();
        self.init();
    })
    //form to JSON
    $.fn.serializeFormJSON = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
})(jQuery);
