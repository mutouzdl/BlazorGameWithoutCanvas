const listeners = {};

addDomEventListener = (element, eventName, dotNetHelper, callbackFuncName) => {
  let dom = document.querySelector(element);

  if (dom) {
    const callback = (e) => {
      listeners[dom.id].dotNetHelper.invokeMethodAsync(callbackFuncName, {
        animationName: e.animationName,
        target: e.target.id,
        type: e.type,
      });
    };

    // 记录事件回调状态
    listeners[dom.id] = {
      callback,
      dom,
      dotNetHelper,
    };

    dom.addEventListener(eventName, callback);
  }
}

removeDomEventListener = (element, eventName) => {
  let dom = document.querySelector(element);
  if (!dom) {
    return;
  }

  const {
    callback,
  } = listeners[dom.id];

  dom.removeDomEventListener(eventName, callback)

  listeners[dom.id] = undefined;
}