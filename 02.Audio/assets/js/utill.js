class Util {
    constructor() {
        if (Util.instance) return Util.instance;
        return this.getInstance(...arguments);
    }

    getInstance() {
        var instance = {
            /*
             *   formatTime 格式化時間（s）為 hour:minutes:seconds
             *   @params  time  required number (s)
             *   
             *   return hour:minutes:seconds string
             */

            formatTime(time) {
                //沒有傳time的時候
                if (time === undefined) {
                    this.handlerError(123, {
                        method: 'formate',
                        param: 'time'
                    });
                    return false;
                }
                let _time = Math.floor(time);
                let _minutes = Math.floor(_time / 60);
                let _hours = Math.floor(_minutes / 60);
                let _seconds = _time - (_minutes * 60);

                return (_hours ? this.fillZero(_hours) + ':' : '') + this.fillZero(_minutes - (_hours * 60)) + ':' + this.fillZero(_seconds);
            },
            /*
             *   fillZero 為小於10的數字補0
             *   @params  num  required number
             *   return '01'.. string
             */
            fillZero(num) {
                //當沒有傳time的時候
                if (num === undefined) {
                    this.handlerError(123, {
                        method: 'fillZero',
                        param: 'num'
                    });
                    return false;
                }
                //這個函數只是讓我們在渲染/顯示的時候有一個不同的效果，不要操作原資料
                return num > 9 ? num : '0' + num;
            },
            errors: {
                123: ({
                    method,
                    param
                }) => {
                    return method + 'function need a param ' + param;
                }
            },
            handlerError(code, options) { //處理報錯
                console.error('[until error] message' + this.errors[code](options));
            }
        }
        Util.instance = instance;
        return instance;
    }
}

//為了這個工具以後在模組化環境中依然可以使用，需要判斷一下，如果是在模組化環境，就將其暴露出去
//commonJs
if (typeof module === 'object' && typeof module.exports === 'object') {
    module.exports = Util;
}

//AMD
if (typeof define === 'function' && define.amd) {
    define('util', [], function () {
        return Util;
    });
}

