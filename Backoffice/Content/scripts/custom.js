// Defines 'console' if the browser does not support it.
if (typeof console == "undefined") {
    this.console = {
        log: function () { }
    };
}

// C#-inspired string formatting
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
    });
};
