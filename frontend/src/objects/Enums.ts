export enum State {
  awaiting = 0,
  editing = 1,
  running = 2,
  canceled = 3,
  completed = 4,
}

export const getStateFromEnum = (state: State): string => {
  switch (state) {
    case State.awaiting: {
      return "Ожидание";
    }
    case State.editing: {
      return "Редактирование";
    }
    case State.running: {
      return "Запущен";
    }
    case State.completed: {
      return "Завершен";
    }
    case State.canceled: {
      return "Отменен";
    }
  }
};
