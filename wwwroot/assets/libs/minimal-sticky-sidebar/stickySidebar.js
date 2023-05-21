(function($) {

    $.fn.stickySidebar = function(options) {
        var opts = $.extend(true, {}, $.fn.stickySidebar.defaults, options);
        var $sidebar = this;
        var $window = $( window );
        var originalOffsetTop = $sidebar.offset().top;

        var getMarginTop = function() {
            if ($window.scrollTop() < originalOffsetTop) { return 0; }
            var maxMarginTop = $( 'body' ).height() - originalOffsetTop - opts.footerThreshold - $sidebar.height();
            return Math.min( maxMarginTop, $window.scrollTop() - originalOffsetTop + opts.topPadding );
        };

        $window.scroll(function() {
            $sidebar.stop().animate( { marginTop: getMarginTop() }, opts.animationDuration );
        });
    };

    $.fn.stickySidebar.defaults = {
        topPadding: 50,
        footerThreshold: 50,
        animationDuration: 300,
    };

})(jQuery);
