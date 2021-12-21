window.test = () => {
  console.log('test!!!');
}

window.addDomEventListener = (element, eventName, dotNetHelper, callbackFuncName) => {
  console.log('addDomEventListener1111,element:', element);

  let dom = document.querySelector(element);
  console.log('dom:', dom);
  if (dom) {
    dom.addEventListener(eventName, (e) => {
      console.log('callbackFuncName:', callbackFuncName, ',e:',e);
      dotNetHelper.invokeMethodAsync(callbackFuncName, {
        animationName: e.animationName,
        target: e.target.id,
        type: e.type,
      });
    });
  }
}
window.addDomEventListener2 = (element, eventName, invoker) => {
  console.log('addDomEventListener!!!!');
  let callback = args => {
    const obj = {};
    for (let k in args) {
      if (k !== 'originalTarget') { //firefox occasionally raises Permission Denied when this property is being stringified
        obj[k] = args[k];
      }
    }
    let json = JSON.stringify(obj, (k, v) => {
      if (v instanceof Node) return 'Node';
      if (v instanceof Window) return 'Window';
      return v;
    }, ' ');
    setTimeout(function () { invoker.invokeMethodAsync('Invoke', json) }, 0);
  };

  if (element == 'window') {
    if (eventName == 'resize') {
      window.addEventListener(eventName, this.debounce(() => callback({ innerWidth: window.innerWidth, innerHeight: window.innerHeight }), 200, false));
    } else {
      window.addEventListener(eventName, callback);
    }
  } else {
    let dom = domInfoHelper.get(element);
    if (dom) {
      dom.addEventListener(eventName, callback);
    }
  }
}