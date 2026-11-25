$(function () {
    const csrfToken = $('meta[name="csrf-token"]').attr('content');
    if (csrfToken) {
        $.ajaxSetup({
            headers: {
                'RequestVerificationToken': csrfToken
            }
        });
    }
});
