function onClickSearchButton(url, searchString, page) {
    window.$.ajax({
        url: url,
        type: "GET",
        data: {
            searchString: page > 1 ? searchString : window.$("#searchText").val(),
            page: page
        },
        success: function (results) {
            window.$("#containerForResults").empty().append(results);
        }
    });
}