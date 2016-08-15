jQuery(function ($) {
    var target = $('#target');

    $('.toggle-loading').click(function () {
        if (target.hasClass('loading')) {
            target.loadingOverlay('remove');
        } else {
            target.loadingOverlay();
        };
    });
});