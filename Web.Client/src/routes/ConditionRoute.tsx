
export function ConditionRoute({ children, condition }: { children: JSX.Element, condition: boolean }) {

  if (!condition) {
    return;
  }

  return children;
}
