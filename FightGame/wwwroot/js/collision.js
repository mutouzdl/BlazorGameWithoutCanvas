
// 是否碰撞
function isCollide(node1, node2) {
  var rect1 = node1[0].getBoundingClientRect();
  var rect2 = node2[0].getBoundingClientRect();
  var overlap = !(rect1.right < rect2.left || rect1.left > rect2.right || rect1.bottom < rect2.top || rect1.top > rect2.bottom);

  return overlap;
}
