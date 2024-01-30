/**
 * Represents the result after a new user is created.
   This interface intentionally does not contain password property for security reasons.
 */
export interface IUserResultModel {
  /// <summary>
  /// Autogenerated ID that uniquely identifies each user with their unique ID.
  /// </summary>
  /// <example>1</example>
  UserID: number;
  /// <summary>
  /// Username of the user.
  /// </summary>
  /// <example>karthik</example>
  Username: string;
  /// <summary>
  /// Email address of the user.
  /// </summary>
  /// <example>karthik@gmail.com</example>
  Email: string;
  /// <summary>
  /// Phone numer of the user.
  /// </summary>
  /// <example>9940568874</example>
  Phone: number;
}