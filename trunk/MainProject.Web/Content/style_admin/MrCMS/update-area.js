(function ($, window) {

    'use strict';

    function updateArea(event, area) {
        var areaName = 'update-' + area;
        var selector = '[data-' + areaName + ']';
        $(selector).each(function () {
            var info = $(this);
            $.get(info.data(areaName), function (result) {
                info.replaceWith(result);
            });
        });
    }

    $(function () {
        $(document).on('update-area', updateArea);
    });

})(jQuery, window);