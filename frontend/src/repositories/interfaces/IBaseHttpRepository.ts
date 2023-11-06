export default interface IBaseHttpRepository<T> {
  getAsync(): Promise<T[] | undefined>;

  postAsync(entity: T): Promise<void>;

  putAsync(entity: T): Promise<void>;

  deleteAsync(id: string): Promise<void>;
}
